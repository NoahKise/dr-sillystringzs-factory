using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Factory.Models;

public class Machine
{
    public int MachineId { get; set; }
    [Required(ErrorMessage = "The machine needs a name")]
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public List<EngineerMachine> EngineerMachines { get; }
}