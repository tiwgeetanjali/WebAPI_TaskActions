using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI_TaskActions.Model
{
	public class Task
	{
        [Key]
        public int task_id { get; set; }
        [Required(ErrorMessage = "Please enter name")]
        public string? task_name { get; set; }
        [Required(ErrorMessage = "Please enter priority (numeric value only)")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Please enter priority (numeric value only)")]
        public int task_priority { get; set; }
        [Required(ErrorMessage = "Please select status")]
        public string? task_status { get; set; }

    }
}

