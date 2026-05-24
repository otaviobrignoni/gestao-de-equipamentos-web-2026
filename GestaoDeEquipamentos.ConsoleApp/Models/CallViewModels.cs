using System.ComponentModel.DataAnnotations;

namespace GestaoDeEquipamentos.ConsoleApp.Models;

public record CallViewModel(
    [Required(ErrorMessage = "O campo \"Título\" é obrigatório.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "O título deve ter entre 2 e 50 caracteres.")]
    string Title,

    [StringLength(500, ErrorMessage = "O campo \"Descrição\" deve conter no máximo 500 caracteres.")]
    string? Description,

    [Required(ErrorMessage = "O campo \"Equipamento\" é obrigatório.")]
    Guid EquipmentId,

    bool IsDone = false,

    Guid Id = default
);

public record CallShowViewModel(
    string Title,
    string? Description,
    string Equipment,
    DateTime OpeningDate,
    int ElapsedTime,
    bool IsDone,
    Guid Id = default
);
