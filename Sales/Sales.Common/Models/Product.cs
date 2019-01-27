namespace Sales.Common.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        [StringLength(50)]
    
        public string Description { get; set; }
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }

        [Display(Name = "Price")]
        [DisplayFormat(DataFormatString ="{0:C2}", ApplyFormatInEditMode =false)]
        public Decimal Prrice { get; set; }


        [Display(Name = "Is Available")]
        public bool IsAvailable { get; set; }

        [Display(Name = "Publish On")]
        [DataType(DataType.Date)]
        public DateTime PublishOn { get; set; }

        [Display(Name = "Image")]
        public string ImagePath { get; set; } 

        public override string ToString()
        {
            return this.Description;
        }

    }
}
