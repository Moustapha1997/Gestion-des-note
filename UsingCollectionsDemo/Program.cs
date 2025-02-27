using System;
using System.Collections;

namespace UsageCollections
{
    class Program
    {
        static void Main(string[] args)
        {
            SortedList listeEtudiants = new SortedList();
            int lignesParPage = 5; // Valeur par défaut
            double sommeMoyennes = 0;
            bool continuer = true;

            // Configurer le nombre de lignes par page
            Console.WriteLine("=== CONFIGURATION DE LA PAGINATION ===");
            Console.Write("Nombre de lignes par page (1-15, défaut 5): ");
            string input = Console.ReadLine();
            if (!string.IsNullOrEmpty(input) && int.TryParse(input, out int nbLignes))
            {
                if (nbLignes >= 1 && nbLignes <= 15)
                {
                    lignesParPage = nbLignes;
                }
                else
                {
                    Console.WriteLine("Valeur incorrecte. Utilisation de la valeur par défaut (5).");
                }
            }

            // Menu principal
            while (continuer)
            {
                Console.Clear();
                Console.WriteLine("=== GESTION DES NOTES DE CLASSE ===");
                Console.WriteLine("1. Ajouter un étudiant");
                Console.WriteLine("2. Afficher la liste des étudiants");
                Console.WriteLine("3. Quitter");
                Console.Write("Votre choix: ");

                string choix = Console.ReadLine();
                switch (choix)
                {
                    case "1":
                        AjouterEtudiant(listeEtudiants, ref sommeMoyennes);
                        break;
                    case "2":
                        AfficherEtudiants(listeEtudiants, sommeMoyennes, lignesParPage);
                        break;
                    case "3":
                        continuer = false;
                        Console.WriteLine("Au revoir!");
                        break;
                    default:
                        Console.WriteLine("Option invalide. Appuyez sur une touche pour continuer...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void AjouterEtudiant(SortedList listeEtudiants, ref double sommeMoyennes)
        {
            Console.Clear();
            Console.WriteLine("=== AJOUT D'UN ÉTUDIANT ===");

            // Saisie du numéro d'ordre
            Console.Write("Numéro d'ordre (NO): ");
            if (!int.TryParse(Console.ReadLine(), out int no))
            {
                Console.WriteLine("Numéro d'ordre invalide. Appuyez sur une touche pour continuer...");
                Console.ReadKey();
                return;
            }

            // Vérifier si le numéro existe déjà
            if (listeEtudiants.ContainsKey(no))
            {
                Console.WriteLine($"Un étudiant avec le NO {no} existe déjà. Appuyez sur une touche pour continuer...");
                Console.ReadKey();
                return;
            }

            // Créer un nouvel étudiant
            Etudiant etudiant = new Etudiant();
            etudiant.NO = no;

            // Saisie du nom
            Console.Write("Nom: ");
            etudiant.Nom = Console.ReadLine();

            // Saisie du prénom
            Console.Write("Prénom: ");
            etudiant.Prénom = Console.ReadLine();

            // Saisie de la note de contrôle continu
            Console.Write("Note de contrôle continu (sur 20): ");
            if (!double.TryParse(Console.ReadLine(), out double noteCC) || noteCC < 0 || noteCC > 20)
            {
                Console.WriteLine("Note invalide (doit être entre 0 et 20). Appuyez sur une touche pour continuer...");
                Console.ReadKey();
                return;
            }
            etudiant.NoteCC = noteCC;

            // Saisie de la note de devoir
            Console.Write("Note de devoir (sur 20): ");
            if (!double.TryParse(Console.ReadLine(), out double noteDevoir) || noteDevoir < 0 || noteDevoir > 20)
            {
                Console.WriteLine("Note invalide (doit être entre 0 et 20). Appuyez sur une touche pour continuer...");
                Console.ReadKey();
                return;
            }
            etudiant.NoteDevoir = noteDevoir;

            // Ajouter l'étudiant à la liste
            listeEtudiants.Add(no, etudiant);

            // Ajouter la moyenne de l'étudiant à la somme des moyennes
            sommeMoyennes += etudiant.CalculerMoyenne();

            Console.WriteLine("Étudiant ajouté avec succès! Appuyez sur une touche pour continuer...");
            Console.ReadKey();
        }

        static void AfficherEtudiants(SortedList listeEtudiants, double sommeMoyennes, int lignesParPage)
        {
            if (listeEtudiants.Count == 0)
            {
                Console.WriteLine("Aucun étudiant dans la liste. Appuyez sur une touche pour continuer...");
                Console.ReadKey();
                return;
            }

            int nombrePages = (int)Math.Ceiling((double)listeEtudiants.Count / lignesParPage);
            int pageActuelle = 1;
            bool continuerAffichage = true;

            while (continuerAffichage && pageActuelle <= nombrePages)
            {
                Console.Clear();
                Console.WriteLine($"=== LISTE DES ÉTUDIANTS (Page {pageActuelle}/{nombrePages}) ===");
                Console.WriteLine("NO  | Nom           | Prénom        | NoteCC  | NoteDevoir | Moyenne");
                Console.WriteLine("----+--------------+--------------+---------+------------+--------");

                // Calculer les indices pour la pagination
                int debut = (pageActuelle - 1) * lignesParPage;
                int fin = Math.Min(debut + lignesParPage, listeEtudiants.Count);

                // Afficher les étudiants de la page courante
                for (int i = debut; i < fin; i++)
                {
                    Etudiant etudiant = (Etudiant)listeEtudiants.GetByIndex(i);
                    Console.WriteLine($"{etudiant.NO,-3} | {etudiant.Nom,-12} | {etudiant.Prénom,-12} | {etudiant.NoteCC,-7:F2} | {etudiant.NoteDevoir,-10:F2} | {etudiant.CalculerMoyenne(),-7:F2}");
                }

                // Afficher la moyenne de la classe à la fin de la dernière page
                if (pageActuelle == nombrePages)
                {
                    double moyenneClasse = listeEtudiants.Count > 0 ? sommeMoyennes / listeEtudiants.Count : 0;
                    Console.WriteLine("\n------------------------------------------------------------");
                    Console.WriteLine($"MOYENNE DE LA CLASSE: {moyenneClasse:F2}/20");
                }

                // Navigation entre les pages
                if (nombrePages > 1)
                {
                    Console.WriteLine("\nNavigation: [P]récédent, [S]uivant, [Q]uitter");
                    Console.Write("Votre choix: ");
                    string navigation = Console.ReadLine().ToUpper();

                    switch (navigation)
                    {
                        case "P":
                            if (pageActuelle > 1)
                                pageActuelle--;
                            break;
                        case "S":
                            if (pageActuelle < nombrePages)
                                pageActuelle++;
                            break;
                        case "Q":
                            continuerAffichage = false;
                            break;
                        default:
                            // Rester sur la même page si choix invalide
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("\nAppuyez sur une touche pour retourner au menu principal...");
                    Console.ReadKey();
                    continuerAffichage = false;
                }
            }
        }
    }
}