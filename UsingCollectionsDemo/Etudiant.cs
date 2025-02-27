using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UsageCollections
{
    public class Etudiant
    {
        public int NO { get; set; }
        public string Nom { get; set; }
        public string Prénom { get; set; }
        public double NoteCC { get; set; }
        public double NoteDevoir { get; set; }

        // Méthode pour calculer la moyenne (33% pour CC, 67% pour Devoir)
        public double CalculerMoyenne()
        {
            return (NoteCC * 0.33) + (NoteDevoir * 0.67);
        }

        // Pour faciliter l'affichage
        public override string ToString()
        {
            return $"NO: {NO}, Nom: {Nom}, Prénom: {Prénom}, NoteCC: {NoteCC}, NoteDevoir: {NoteDevoir}, Moyenne: {CalculerMoyenne():F2}";
        }
    }
}