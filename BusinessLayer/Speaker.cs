using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLayer
{
    /// <summary>
    /// Represents a single speaker
    /// </summary>
    public class Speaker
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public int? yearExperience { get; set; }
		public bool HasBlog { get; set; }
		public string BlogURL { get; set; }
		public WebBrowser Browser { get; set; }
		public List<string> Certifications { get; set; }
		public string Employer { get; set; }
		public int RegistrationFee { get; set; }
		public List<BusinessLayer.Session> Sessions { get; set; }

		/// <summary>
		/// Register a speaker
		/// </summary>
		/// <returns>speakerID</returns>
		public int? Register(IRepository repository)
		{
            validarRegistro();
            int registrationFee = repository.ObtenerRegistrationFee(yearExperience);
            int speakerID = repository.SaveSpeaker(this);
            return speakerID;
		}

        private void validarRegistro()
        {
            validarCamposLlenos();
            if (!cumpleRequisitos())
            {
                throw new SpeakerDoesntMeetRequirementsException("Speaker doesn't meet our abitrary and capricious standards.");
            }
            Aprobado();
        }

        private void validarCamposLlenos()
        {
            if (string.IsNullOrEmpty(FirstName)) throw new ArgumentNullException("First Name is required.");
            if (string.IsNullOrEmpty(LastName)) throw new ArgumentNullException("Last Name is required.");
            if (string.IsNullOrEmpty(Email)) throw new ArgumentNullException("Email is required.");
            if (Sessions.Count() == 0) throw new ArgumentException("Can't register speaker with no sessions to present.");
        }

        private bool cumpleRequisitos()
        {
            return esExperto() || !validaMailyNavegador();
        }
        
        private bool esExperto()
        {
            if (yearExperience > 10) return true;
            if (HasBlog) return true;            
            if (Certifications.Count() > 3) return true;
            if (buenEmpleo()) return true;
            return false;
        }

        private bool buenEmpleo()
        {
            string[] empresas = new string[] { "Microsoft", "Google", "Fog Creek Software", "37Signals" };
            return empresas.Contains(Employer);
        }

        private bool validaMailyNavegador()
        {
            return antiguoDominio() || antiguoNavegador();
        }

        private bool antiguoDominio()
        {
            string[] listaEmailDomain = new string[] { "aol.com", "hotmail.com", "prodigy.com", "CompuServe.com" };
            string emailDomain = Email.Split('@').Last();
            return listaEmailDomain.Contains(emailDomain);
        }

        private bool antiguoNavegador()
        {
            return Browser.Name == WebBrowser.BrowserName.InternetExplorer && Browser.MajorVersion < 9;
        }

        private void Aprobado()
        {
            foreach (var session in Sessions)
            {
                session.Approved = !EsAntiguaTecnologia(session.Description);
            }

            bool noSessionsApproved = Sessions.Where(s => s.Approved).Count() == 0;
            if (noSessionsApproved) throw new NoSessionsApprovedException("No sessions approved.");
        }
                       
        private bool EsAntiguaTecnologia(string sessionDescription)
        {
            string[] oldTechnologies = new string[] { "Cobol", "Punch Cards", "Commodore", "VBScript" };
            foreach (var oldTech in oldTechnologies)
            {
                if (sessionDescription.Contains(oldTech)) return true;
            }
            return false;
        }

        #region Custom Exceptions
        public class SpeakerDoesntMeetRequirementsException : Exception
		{
			public SpeakerDoesntMeetRequirementsException(string message)
				: base(message)
			{
			}

			public SpeakerDoesntMeetRequirementsException(string format, params object[] args)
				: base(string.Format(format, args)) { }
		}

		public class NoSessionsApprovedException : Exception
		{
			public NoSessionsApprovedException(string message)
				: base(message)
			{
			}
		}
		#endregion
	}
}