﻿using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Infrastructure.Repository;

internal class ReviewAssignmentRepository : GenericRepository<AssignmentPeerReview>, IReviewAssignmentRepository
{
    private readonly ApplicationDbContext _context;
    public ReviewAssignmentRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
}
