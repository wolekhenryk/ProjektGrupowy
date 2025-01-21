﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace ProjektGrupowy.API.Models;

[Table("Labels")]
public class Label
{
    [Key]
    public int Id { get; set; } 

    public string Name { get; set; }
    
    public string ColorHex { get; set; }
    
    public string Type { get; set; } // consider enum, ask about it, in the json given to us all types are range

    public char Shortcut { get; set; }
    
    [Required]
    public virtual Subject Subject { get; set; }

    public virtual ICollection<AssignedLabel>? AssignedLabels { get; set; }

    public string ToJson()
    {
        var jsonObject = new
        {
            label = new
            {
                name = Name,
                color = ColorHex,
                type = Type,
                shortcut = Shortcut.ToString()
            }
        };

        return JsonSerializer.Serialize(jsonObject, new JsonSerializerOptions
        {
            WriteIndented = true
        });
    }
}