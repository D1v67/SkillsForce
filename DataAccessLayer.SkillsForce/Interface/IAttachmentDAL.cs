﻿using Common.SkillsForce.Entity;
using System.Collections.Generic;

namespace DataAccessLayer.SkillsForce.DAL
{
    public interface IAttachmentDAL
    {
        void Add(AttachmentModel attachment);
        IEnumerable<AttachmentModel> GetAll();
        IEnumerable<AttachmentModel> GetAllByEnrollmentID(int id);
        AttachmentModel GetByAttachmentID(int id);
    }
}