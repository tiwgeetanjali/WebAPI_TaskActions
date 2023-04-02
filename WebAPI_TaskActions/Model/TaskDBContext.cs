using System;
using Microsoft.EntityFrameworkCore;

namespace WebAPI_TaskActions.Model
{
	public class TaskDBContext : DbContext
	{
		public TaskDBContext(DbContextOptions options):base(options)
		{

		}

		public DbSet<Task> Task { get; set; }
	}
}

