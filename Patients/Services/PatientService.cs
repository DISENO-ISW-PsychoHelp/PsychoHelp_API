﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PsychoHelp_API.Domain.Repositories;
using PsychoHelp_API.patients.Domain.Models;
using PsychoHelp_API.patients.Domain.Repositories;
using PsychoHelp_API.patients.Domain.Services;
using PsychoHelp_API.patients.Domain.Services.Communication;

namespace PsychoHelp_API.patients.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PatientService(IUnitOfWork unitOfWork, IPatientRepository patientRepository)
        {
            _unitOfWork = unitOfWork;
            _patientRepository = patientRepository;
        }

        public async Task<IEnumerable<Patient>> ListAsync()
        {
            return await _patientRepository.ListAsync();
        }

        public async Task<PatientResponse> SaveAsync(Patient patient)
        {

            try
            {
                await _patientRepository.AddSync(patient);
                await _unitOfWork.CompleteAsync();

                return new PatientResponse(patient);
            }
            catch (Exception e)
            {
                return new PatientResponse($"An error occurred while saving the patient {e.Message}");
            }
        }

        public async Task<PatientResponse> UpdateAsync(int id, Patient patient)
        {
            var existingPatient = await _patientRepository.FindByIdAsync(id);

            if (existingPatient == null)
                return new PatientResponse("Product not found");

            existingPatient.FirstName = patient.FirstName;
            existingPatient.LastName = patient.LastName;
            existingPatient.Email = patient.Email;
            existingPatient.Gender = patient.Gender;
            existingPatient.Img = patient.Img;
            existingPatient.Password = patient.Password;
            existingPatient.Phone = patient.Phone;
            existingPatient.State = patient.State;

            try
            {
                _patientRepository.Update(existingPatient);
                await _unitOfWork.CompleteAsync();

                return new PatientResponse(existingPatient);
            }
            catch (Exception e)
            {
                return new PatientResponse($"An error occurred while updating the patient {e.Message}");
            }
        }

        public async Task<PatientResponse> DeleteAsync(int id)
        {
            var existingPatient = await _patientRepository.FindByIdAsync(id);

            if (existingPatient == null)
                return new PatientResponse("Patient not found");

            try
            {
                _patientRepository.Remove(existingPatient);
                await _unitOfWork.CompleteAsync();

                return new PatientResponse(existingPatient);
            }
            catch (Exception e)
            {
                return new PatientResponse($"An error occurred while deleting the product {e.Message}");
            }
        }

        public Task<Patient> GetByIdAsync(int id)
        {
            return _patientRepository.FindByIdAsync(id);
        }

        public Task<Patient> GetByEmailAsync(string email)
        {
            return _patientRepository.FindByEmailAsync(email);
        }
    }
}