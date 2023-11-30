using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace course_work_server.Entities;

public class Race
{
    public int Id { get; set; }
	[Required]
	public string Title { get; set; }
	public string Location { get; set; }
    public DateTime Date { get; set; }


	public ICollection<User> Users { get; set; }
	public Race()
	{
		Users = new List<User>();
	}

}