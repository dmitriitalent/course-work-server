using System.ComponentModel.DataAnnotations;

namespace course_work_server.Entities;

public class Race
{
    public int Id { get; set; }
	[Required]
	public string Title { get; set; }
	public string Location { get; set; }
    public DateTime Date { get; set; }
    
}