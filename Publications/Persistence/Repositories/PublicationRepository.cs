﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PsychoHelp_API.Persistence.Contexts;
using PsychoHelp_API.Persistence.Repositories;
using PsychoHelp_API.Publications.Domain.Models;
using PsychoHelp_API.Publications.Domain.Repositories;

namespace PsychoHelp_API.Publications.Persistence.Repositories
{
    public class PublicationRepository : BaseRepository, IPublicationRepository
    {
        public PublicationRepository(AppDbContext context) : base(context)
        {
        }

        public async Task AddAsync(Publication publication)
        {
            await _context.Publications.AddAsync(publication);
        }

        public async Task<Publication> FindByIdAsync(int id)
        {
            return await _context.Publications
                .Include(p => p.Psychologist)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Publication>> FindByPsychologistIdAsync(int psychologistId)
        {
            return await _context.Publications
                .Where(p => p.PsychologistId == psychologistId)
                .Include(p => p.Psychologist)
                .ToListAsync();
        }

        public async Task<IEnumerable<Publication>> ListAsync()
        {
            return await _context.Publications
                .Include(p => p.Psychologist)
                .ToListAsync();
        }

        public void Remove(Publication publication)
        {
            _context.Publications.Remove(publication);
        }

        public void Update(Publication publication)
        {
            _context.Publications.Update(publication);
        }
    } 
}