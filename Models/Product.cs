using System;
using System.ComponentModel.DataAnnotations;

namespace _22DH112015_TranPhiLong.Models;

public partial class Product
{
    public int ProductId { get; set; }

    [Required(ErrorMessage = "Tên sản phẩm là bắt buộc")]
    public string? NamePro { get; set; }

    public string? DecriptionPro { get; set; }

    [Required(ErrorMessage = "Danh mục là bắt buộc")]
    [RegularExpression("^(Vợt|Bóng|Cầu|Đệm|Quần áo)$", ErrorMessage = "Danh mục phải là Vợt, Bóng, Cầu, Đệm hoặc Quần áo")]
    public string? Category { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Giá phải là số dương")]
    public decimal? Price { get; set; }

    public string? ImagePro { get; set; }

    [Required(ErrorMessage = "Ngày sản xuất là bắt buộc")]
    [DataType(DataType.Date)]
    [CustomValidation(typeof(Product), nameof(ValidateManufacturingDate))]
    public DateOnly ManufacturingDate { get; set; }

    public static ValidationResult ValidateManufacturingDate(DateOnly date)
    {
        return date <= DateOnly.FromDateTime(DateTime.Today)
            ? ValidationResult.Success
            : new ValidationResult("Ngày sản xuất không được trong tương lai");
    }
}