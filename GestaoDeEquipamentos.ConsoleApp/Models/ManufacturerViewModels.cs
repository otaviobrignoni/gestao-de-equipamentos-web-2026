using System.ComponentModel.DataAnnotations;

namespace GestaoDeEquipamentos.ConsoleApp.Models;

public record ManufacturerViewModel(
        [Required(ErrorMessage = "O campo \"Nome\" é obrigatório.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "O nome deve ter entre 2 e 50 letras.")]
        string Name,

        [Required(ErrorMessage = "O campo \"Email\" é obrigatório.")]
        [EmailAddress(ErrorMessage = "Email inválido.")]
        string Email,

        [Required(ErrorMessage = "O campo \"Telefone\" é obrigatório.")]
        [RegularExpression(@"^\(\d{2}\)\s(9?\d{4})-\d{4}$", ErrorMessage = "Telefone no formato inválido")]
        string PhoneNumber,

        Guid Id = default
);