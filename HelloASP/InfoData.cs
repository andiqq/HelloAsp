using System;
using Microsoft.EntityFrameworkCore;


public class InfoData
{
    public int Id { get; set; }
    public string? name { get; set; }
}

public class InfoDataContext : DbContext
{
    public InfoDataContext(DbContextOptions<InfoDataContext> options) : base(options)
    {

    }

    public DbSet<InfoData>? infoDatas => Set<InfoData>();
}


