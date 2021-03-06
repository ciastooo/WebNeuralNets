﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebNeuralNets.Models.DB
{
    public class Neuron
    {
        public Neuron()
        {
            PreviousDendrites = new HashSet<Dendrite>();
            NextDendrites = new HashSet<Dendrite>();
        }

        [Key]
        public int Id { get; set; }

        public int LayerId { get; set; }

        public double Bias { get; set; }

        [NotMapped]
        public double Delta { get; set; }

        [NotMapped]
        public double Value { get; set; }

        public ICollection<Dendrite> PreviousDendrites { get; set; }
        public ICollection<Dendrite> NextDendrites { get; set; }
        public Layer Layer { get; set; }
    }
}
