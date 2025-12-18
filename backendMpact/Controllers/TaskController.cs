using backendMpact.Data;
using backendMpact.DTO;
using backendMpact.Models;
using backendMpact.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Text.Json;

[Route("api/[controller]")]
[ApiController]
public class TasksController : ControllerBase
{
    private readonly AppDbContext _context;

    private readonly IUserService _service;
    private readonly IEmailService _emailService;

    public TasksController(AppDbContext context, IEmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }
   

    // CREATE TASK (Status is automatically pending)
    [HttpPost("add")]
    public async Task<IActionResult> AddTask([FromBody] CreateTaskDto dto)
    {
        // Convert AgentId
        if (!int.TryParse(dto.AgentId, out int agentId))
            return BadRequest("Invalid AgentId format.");

        // Convert AssignedBy
        if (!int.TryParse(dto.AssignedBy, out int assignedById))
            return BadRequest("Invalid AssignedBy format.");


        // Validate agent (Inspector)
        var inspector = await _context.Users
            .FirstOrDefaultAsync(u =>
                u.Id == agentId &&
                u.Role == "Inspector");

        if (inspector == null)
            return BadRequest("Invalid AgentId. Selected user is not an Inspector.");


        // Validate manager (AssignedBy)
        var manager = await _context.Users
            .FirstOrDefaultAsync(u =>
                u.Id == assignedById &&
                u.Role == "Manager");

        if (manager == null)
            return BadRequest("Invalid AssignedBy. Selected user is not a Manager.");




        var task = new TaskItem
        {
            Title = dto.Title,
            Description = dto.Description,
            AgentId = dto.AgentId,
            AssignedBy = dto.AssignedBy,
            Priority = dto.Priority,
            DueDate = dto.DueDate,
            Status = "pending",
            CreatedAt = DateTime.UtcNow
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        return Ok(task);
    }

    [HttpPut("inspector-update/{id}")]
    public async Task<IActionResult> InspectorUpdateTask(
    int id,
    [FromForm] InspectorUpdateTaskDto dto,
    [FromServices] FileService fileService)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null)
            return NotFound("Task not found");

        task.InspectorNotes = dto.Notes;

       
        if (dto.PdfFile != null)
            task.PdfFile = await fileService.SaveFileToDesktop(dto.PdfFile, id, true);

       
        var imagePaths = new List<string>();

        if (dto.Images != null && dto.Images.Count > 0)
        {
            foreach (var image in dto.Images)
            {
                string savedImagePath = await fileService.SaveFileToDesktop(image, id);
                imagePaths.Add(savedImagePath);
            }
        }

        
        task.ImagePathsJson = JsonSerializer.Serialize(imagePaths);

        task.Status = "submitted";
        task.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(task);
    }


    [HttpPut("manager-review/{id}")]
    public async Task<IActionResult> ManagerReviewTask(int id, [FromBody] ManagerReviewDto dto)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null)
            return NotFound("Task not found");

        // Update manager comments and review timestamp
        task.ManagerComments = dto.Comments;
        task.ReviewedAt = DateTime.UtcNow;

        if (dto.IsApproved)
        {
            task.IsApproved = true;
            task.Status = "completed";
            task.ExternalRecipients = dto.ExternalRecipients;

            // Build attachment list: PDF + images
            var attachments = new List<string>();
            if (!string.IsNullOrEmpty(task.PdfFile))
                attachments.Add(task.PdfFile);

            var images = JsonSerializer.Deserialize<List<string>>(task.ImagePathsJson) ?? new List<string>();
            attachments.AddRange(images);

            // Send email to external recipients
            if (!string.IsNullOrWhiteSpace(dto.ExternalRecipients))
            {
                var recipients = dto.ExternalRecipients.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                foreach (var email in recipients)
                {
                    await _emailService.SendEmailAsync(
                        email,
                        $"Inspection Task Approved: {task.Title}",
                        $"Inspector completed inspection for task '{task.Title}'.\n\nNotes: {task.InspectorNotes}",
                        attachments
                    );
                }
            }
        }
        else
        {
            // Reject → send back to inspector
            task.IsApproved = false;
            task.Status = "pending"; // or "rejectedByManager"
        }

        task.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return Ok(new
        {
            task.Id,
            task.Title,
            task.Status,
            task.IsApproved,
            task.ManagerComments
        });
    }



[HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        var tasks = await _context.Tasks.ToListAsync();
        return Ok(tasks);
    }

    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null) return NotFound("Task not found");

        return Ok(task);
    }


    [HttpGet("/get/{id}")]
    public async Task<IActionResult> GetTask(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null)
            return NotFound("Task not found");

        // Deserialize images
        var images = JsonSerializer.Deserialize<List<string>>(task.ImagePathsJson) ?? new List<string>();

        return Ok(new
        {
            task.Id,
            task.Title,
            task.Description,
            task.InspectorNotes,
            task.PdfFile,
            Images = images,
            task.Status,
            task.ManagerComments
        });
    }



    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null) return NotFound("Task not found");

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();

        return Ok("Deleted successfully");
    }
}
