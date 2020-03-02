using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NasaPicturesAPI.Models
{
    public class HostedImageResponse : ApiResponse
    {
        public HostedImageResponse()
        {
            //Let's avoid these being null so we can just add to them
            errors = new List<apiError>();
            hostedImages = new List<ExternalHostedImage>();
        }
        public List<ExternalHostedImage> hostedImages { get; set; } 
    }
    public class ExternalHostedImage 
    {
        //public Guid Id { get; set; }
        public string URL { get; set; }
        public string Description { get; set; }
        //public object ExternalReferenceID { get; set; }
        //public Type ExternalReferenceType { get; set; }
    }
    //public class HostImgDBContext : DbContext
    //{
    //    public HostImgDBContext(DbContextOptions<HostImgDBContext> options) : base(options) { }
    //    public DbSet<ExternalHostedImage> ExternalHostedImages { get; set; }

    //    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //    //{
    //    //    modelBuilder.Entity<ExternalHostedImage>()
    //    //        .HasNoKey();
    //    //}
    //}
}
