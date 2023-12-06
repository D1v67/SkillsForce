﻿using Common.SkillsForce.Entity;
using Common.SkillsForce.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.SkillsForce.Interface
{
    public interface IEnrollmentDAL
    {
        IEnumerable<EnrollmentViewModel> GetAll();
        EnrollmentViewModel GetByID(int id);
        void Add(EnrollmentViewModel enrollment);
        void Delete(int id);
        void Update(EnrollmentViewModel enrollment);
        IEnumerable<EnrollmentViewModel> GetAllEnrollmentsWithDetails();
        IEnumerable<EnrollmentViewModel> GetAllEnrollmentsWithDetailsByManager(int managerId);
    }
}
