using System.ComponentModel.DataAnnotations;
using MangaScans.Domain.Entities;

namespace MangaScans.Application.DTOs.Request;

public class MangaDtoRequest
{
    public string Name { get; set; }
    
    public string Description { get; set; }
}