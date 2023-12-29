﻿using Common.SkillsForce.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Common.SkillsForce.ViewModel
{
    public class TrainingViewModel
    {
        [Key]
        public int TrainingID { get; set; }
        public string TrainingName { get; set; }
        public string TrainingDescription { get; set; }
        public DateTime RegistrationDeadline { get; set; }
        public DateTime StartDate { get; set; }
        public int Capacity { get; set; }
        public int DepartmentID { get; set; }
        public List<PrerequisiteModel> Prerequisites { get; set; }
        public List<DepartmentModel> Departments { get; set; }
    }
}
