namespace Sales.Common.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

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

        [NotMapped]
        public byte[] ImageArray { get; set; }

        [Display(Name = "Image")]
        public string ImagePath { get; set; } 

        public string ImageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(this.ImagePath))
                {
                    return "dummyp";
    
                }
                return $"http://192.168.0.100:9000/SaleAPI/{this.ImagePath.Substring(1)}";
            }
        }

        public override string ToString()
        {
            return this.Description;
        }

    }
}
