using System.ComponentModel.DataAnnotations;

namespace GestaoDeEquipamentos.ConsoleApp.Models;

public record EquipmentViewModel(
        [Required(ErrorMessage = "O campo \"Nome\" é obrigatório.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "O nome deve ter entre 2 e 50 letras.")]
        string Name,
        //decimal.MaxValue = 79228162514264337593543950335
        [Range(typeof(decimal), "0.01", "79228162514264337593543950335", ErrorMessage = "O preço de aquisição deve ser positivo.")]
        decimal Price,

        [Required(ErrorMessage = "O campo \"Data de Fabricação\" é obrigatória.")]
        [DataType(DataType.Date)]
        DateOnly Date,
        
        [Required(ErrorMessage = "O campo \"Fabricante\" é obrigatório.")]
        Guid ManufacturerId,

        Guid Id = default
);

public record EquipmentShowViewModel(
        string Name,
        decimal Price,
        DateOnly Date,
        string Manufacturer,
        Guid Id
);