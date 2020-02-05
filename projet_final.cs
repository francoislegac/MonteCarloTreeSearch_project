using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading.Tasks;



namespace projet
{
	public enum Resultat { j1gagne, j0gagne, partieNulle, indetermine }

    public abstract class Position
    {
        public bool j1aletrait;
        public Position(bool j1aletrait) { this.j1aletrait = j1aletrait; }
        public Resultat Eval { get; protected set; }
        public int NbCoups { get; protected set; }
        public int[,] tab { get; protected set; }
        public abstract void EffectuerCoup(int i);
        public abstract Position Clone();
        public abstract void Affiche();
    }

    public abstract class Joueur
    {
        public abstract int Jouer(Position p);
        public virtual void NouvellePartie() { }
    }

    public class Program 
    {
        static void Main() 
        {
            Console.WriteLine("Entrer le numéro de votre choix : \n ");
            Console.WriteLine("1_ JoueurHumain vs JMCTS ALLUMETTES");
            Console.WriteLine("2_ JMCTS vs JMCTS ALLUMETTES");
            Console.WriteLine("3_ JoueurHumain vs JMCTS P4");
            Console.WriteLine("4_ JMCTS vs JMCTS P4");
            Console.WriteLine("5_ (Q7) a Empirique JMCTS (on trouve X)"); 
            Console.WriteLine("6_ (Q8) JMCTS vs JMCTSp");
            Console.WriteLine("7_ (Q9) a Empirique JMCTSp (on trouve X)");
            Console.WriteLine("8_ (Q10 & 11) ");

            int saisie = Int32.Parse(Console.ReadLine());

            switch(saisie)
            {
                case 1:
                    Console.WriteLine("1_ JoueurHumain vs JMCTS ALLUMETTES \n");
                    JoueurHumainA j0 = new JoueurHumainA(); 
                    JMCTS j1 = new JMCTS(20, 30, 10);
                    Console.WriteLine("Veuillez saisir le nombre total d'allumettes pour commencer le jeu");
                    int N  = int.Parse(Console.ReadLine());
                    PositionA positionA = new PositionA(true, N);
                    Partie partieA = new Partie(j1, j0, positionA);
                    partieA.Commencer();
                    break;
                case 2:
                    Console.WriteLine("2_ JMCTS vs JMCTS ALLUMETTES \n");
                    JMCTS j2 = new JMCTS(20, 30, 10);
                    JMCTS j3 = new JMCTS(10, 10, 10);
                    Console.WriteLine("Veuillez saisir le nombre total d'allumettes pour commencer le jeu");
                    int N2  = int.Parse(Console.ReadLine());
                    PositionA positionA2 = new PositionA(true, N2);
                    Partie partieA2 = new Partie(j3, j2, positionA2);
                    partieA2.Commencer();
                    break; 
                case 3:
                    Console.WriteLine("3_ JoueurHumain vs JMCTS P4 \n");
                    JoueurHumainPuissance j4 = new JoueurHumainPuissance();
                    JMCTS j5 = new JMCTS(10,10,100);
                    PositionP4 positionP4 = new PositionP4(false);
                    Partie partieP4 = new Partie(j5, j4, positionP4);
                    partieP4.Commencer();
                    break;
                case 4:
                    Console.WriteLine("4_ JMCTS vs JMCTS P4 \n");
                    JMCTS j6 = new JMCTS(10,10,100);
                    JMCTS j7 = new JMCTS(10,10,100);
                    PositionP4 positionP42 = new PositionP4(false);
                    Partie partieP42 = new Partie(j7, j6, positionP42);
                    partieP42.Commencer();
                    break;
                case 5:
                    Console.WriteLine("5_ (Q7) a Empirique JMCTS (on trouve 10) \n");
                    JMCTS[] tabJMCTS = new JMCTS[100];
                    for(int i=0; i < 100; i++)
                        tabJMCTS[i] = new JMCTS(i, i, 100);
                    Championnat(true, 100, tabJMCTS);
                    break;
                case 6:
                    Console.WriteLine("6_ (Q8) JMCTS vs JMCTSp, on joue 20 parties\n");
                    JMCTS jJMCTS = new JMCTS(10,10,100);
                    JMCTSp jJMCTSp = new JMCTSp(10,10,100,4);
                    PositionP4 positionP43 = new PositionP4(true);
                    LancerPartie(jJMCTSp, jJMCTS, positionP43, 20);
                    break;
                case 7:
                    Console.WriteLine("7_ (Q9) a Empirique JMCTSp (on trouve 14) \n");
                    JMCTSp[] tabJMCTSp = new JMCTSp[100];
                    for(int i=0; i < 100; i++)
                        tabJMCTSp[i] = new JMCTSp(i, i, 100, 4);
                    Championnat(true, 100, tabJMCTSp);                    
                    break;
                case 8:
                    //

                default:
                    Console.WriteLine("test");
                    break;
            }
        } 

        static void Championnat(bool j1aletrait, int N, Joueur[] tabJoueurs)
        {
            int[] winCount = new int[N];
            for(int i =0; i < N; i++)
                winCount[i] = 0;                

            for(int i=0; i < N; i++) 
            {
                for(int j=0; j < N; j++)
                {
                    if(i==j)
                        winCount[i] = 0;
                    else
                    {
                        PositionP4 position = new PositionP4(j1aletrait);
                        Partie partie = new Partie(tabJoueurs[i], tabJoueurs[j], position);
                        //partie.NouveauMatch(position);
                        partie.Commencer();
                        if(partie.r.ToString() == "j0gagne")
                            winCount[i]+= 2;
                        else if(partie.r.ToString() == "partieNulle")
                            winCount[i]+= 1;
                        else
                            winCount[i] += 0;                         
                    }
                }
            }
            int a =0;
            foreach(int x in winCount)
                Console.WriteLine("paramètre a = {0}, nb victoires = {1}", a++, x);
        }        

        static void LancerPartie(Joueur j1, Joueur j0, Position p, int N)
        {
            int reJ0=0;
            int reJ1=0;
            Partie partie = new Partie(j1, j0, p);
            for(int i=0; i < N; i++)
            {
                partie.NouveauMatch(p);
                partie.Commencer();            
                if(partie.r.ToString() == "j0gagne") reJ0+=1;
                if (partie.r.ToString() == "j1gagne") reJ1+=1;
            }
            Console.WriteLine("score JMCTS = " + reJ0 + ", score JMCTS.p = " + reJ1);
        }

    }

public class PositionP4 : Position
{ 

    public PositionP4(bool j1aletrait):base(j1aletrait)
    {
        this.j1aletrait = j1aletrait;
        tab = new int[7,7]; //+1 ligne & +1 colonne pour les indexes
        Eval = Resultat.indetermine;
        NbCoups = 7;
        for(int j=0; j < 7; j++)
            tab[0,j] = j + 1;
    }

    public override void EffectuerCoup(int j) //on a changé i par j pour désigner les colonnes
    {
        if(Eval == Resultat.indetermine) 
        {                        
            while(tab[1,j]!=0) //on vérifie si les colonnes sont remplies
                j++;

            int i = 1;
            while(tab[i,j] == 0 && i + 1 < 7)
                i++;
            if(i==6 && tab[i,j] == 0) //cas où l'on arrive à la dernière case
                tab[i,j] = j1aletrait?2:1;   
            else 
            {
                //cas où la colonne est à un jeton de se remplir complètement
                if(i==2) 
                {
                    --NbCoups;
                    tab[i-1,j] = j1aletrait?2:1;
                    --i;
                }
                else { //cas normal
                    tab[i-1,j] = j1aletrait?2:1;
                    --i;
                }
            }                                

            //vérifications du résultat
            if(VerifierGagner(tab, i, j, j1aletrait) == 2) 
            {
                Eval = Resultat.j1gagne;
                NbCoups = 0;
            }
            else if(VerifierGagner(tab, i, j, j1aletrait) == 1) 
            {
                Eval = Resultat.j0gagne;
                NbCoups = 0;
            }

            else if(VerifierPartieNulle(tab)) 
            {
                Eval= Resultat.partieNulle;
                NbCoups = 0;
            }
            else
                Eval = Resultat.indetermine;
        }
        j1aletrait = !j1aletrait; //on indique que c'est au joueur suivant de jouer
    }

    public int VerifierGagner(int[,] tab, int i, int j, bool j1aletrait) //1 (j0) ou 2 (j1) ou 0 (aucun) 
    {        
        int num = (j1aletrait)?2:1;
        int tmpi = i, tmpj= j;

        //Vérification horizontale
        int count = 0;
        for(int cur=0; cur < 4; cur ++) // déplacement curseur
        {
            for(int z=0; z <4; z++) //test
            {
                if(tab[i, cur + z] == num)
                    ++count;
                if(count ==4)
                    return num;
            }
            count =0;
        }

        //Vérification verticale
        for(int cur=1; cur < 4; cur++) //déplacement curseur
        {
            for(int z=0; z <4 ; z++) //test
            {
                if(tab[cur + z, j] == num)
                    ++count;
                if(count ==4)
                    return num;
            }
            count=0;
        }
        
        //Vérification diagonale : gauche / droite
        i=tmpi;
        j=tmpj;
        count=0;
        while(i != 6 && j != 0) //on ramène le curseur au bord
        {
            i++;
            j--;             
        }

        while(i >3 && j<4) //délimitation zone du curseur
        {
            for(int z=0; z <4; z++) //test
            {
                if(tab[i-z,j+z]==num)
                    count++;
                if(count==4)
                    return num;
            }
            count =0;
            i--;
            j++;
        }
        
        //Vérification diagonale : droite / gauche
        i=tmpi;
        j=tmpj;
        count=0;
        while(i != 6 && j != 6)
        {
            i++;
            j++;
        }

        while(i>3 && j>2) //délimitation zone du curseur
        {
            for(int z=0; z < 4; z++)
            {
                if(tab[i-z, j-z] == num)
                    count++;
                if(count==4)
                    return num;
            }
            count=0;
            i--;
            j--;
        }

        return 0;
    }

    public bool VerifierPartieNulle(int[,] tab) {
        int count =0;
        for(int j=0; j < 7; j++)
            if(tab[1,j] != 0) count ++;
        if(count == 7)
            return true;
        else 
            return false;
    }

    public override Position Clone()
    {
        PositionP4 clonePosition = new PositionP4(base.j1aletrait);
        
        for(int i=0; i<7;i++)
        {
            for(int j=0; j<7; j++)
                clonePosition.tab[i,j] = base.tab[i,j]; 
        }        
        clonePosition.Eval = base.Eval;
        clonePosition.NbCoups = base.NbCoups;
        return clonePosition;
    }

    public override void Affiche()
    {
        string s = "";
        for(int i=0; i<7; i++)
        {
            s+= "\n";
            for(int j=0; j <7; j++)
                s+= tab[i,j] + " | ";
        }
        Console.WriteLine(s);
        if(Eval == Resultat.indetermine)
            Console.WriteLine("C'est au joueur {0} de jouer", (j1aletrait)?1:0);
    }
}


public class JoueurHumainPuissance : Joueur
{
    public override int Jouer(Position p)
    {        
        int saisie=0; 
        Console.WriteLine("Veuillez entrer le numéro de la colonne :");
        saisie = int.Parse(Console.ReadLine());
        while(saisie < 1 || saisie > p.NbCoups) 
        {
            Console.WriteLine("Saisie non autorisée.");
            saisie = int.Parse(Console.ReadLine());
        }
        return saisie - 1;
    }
}


public class Partie
    {
        Position pCourante;
        Joueur j1, j0;
        public Resultat r;

        public Partie(Joueur j1, Joueur j0, Position pInitiale)
        {
            this.j1 = j1;
            this.j0 = j0;
            pCourante = pInitiale.Clone();
        }

        public void NouveauMatch(Position pInitiale)
        {
            pCourante = pInitiale.Clone();
        }

        public void Commencer(bool affichage = true)
        {
            j1.NouvellePartie();
            j0.NouvellePartie();
            do
            {
                if (affichage) pCourante.Affiche();/////////////////////
                if (pCourante.j1aletrait)
                {
                    pCourante.EffectuerCoup(j1.Jouer(pCourante.Clone()));
                }
                else
                {
                    pCourante.EffectuerCoup(j0.Jouer(pCourante.Clone()));
                }
            } while (pCourante.NbCoups>0);
            r = pCourante.Eval;
            if (affichage)
            {
                pCourante.Affiche(); ///////////////////////////////
                switch (r)
                {
                    case Resultat.j1gagne: Console.WriteLine("j1 {0} a gagné.", j1); break;
                    case Resultat.j0gagne: Console.WriteLine("j0 {0} a gagné.",j0); break;
                    case Resultat.partieNulle: Console.WriteLine("Partie nulle."); break;
                }
            }
        }
    }
    
    public class Noeud
    {
        static Random gen = new Random();

        public Position p;
        public Noeud pere;
        public Noeud[] fils;
        public int cross, win;
        public int indiceMeilleurFils;

        public Noeud(Noeud pere, Position p)
        {
            this.pere = pere;
            this.p = p;
            fils = new Noeud[this.p.NbCoups];
        }

        public void CalculMeilleurFils(Func<int, int, float> phi)
        {
            float s;
            float sM = 0;
            if (p.j1aletrait)
            {
                for (int i = 0; i < fils.Length; i++)
                {
                    if (fils[i] == null) { s = phi(0, 0); }
                    else { s = phi(fils[i].win, fils[i].cross); }
                    if (s > sM) { sM = s; indiceMeilleurFils = i; }
                }
            }
            else
            {
                for (int i = 0; i < fils.Length; i++)
                {
                    if (fils[i] == null) { s = phi(0, 0); }
                    else { s = phi(fils[i].cross-fils[i].win, fils[i].cross); }
                    if (s > sM) { sM = s; indiceMeilleurFils = i; }
                }
            }
        }


        public Noeud MeilleurFils()
        {
            if (fils[indiceMeilleurFils] != null)
            {
                return fils[indiceMeilleurFils];
            }
            Position q = p.Clone();
            q.EffectuerCoup(indiceMeilleurFils);
            fils[indiceMeilleurFils] = new Noeud(this, q);
            return fils[indiceMeilleurFils];
        }

        public override string ToString()
        {
            string s = "";
            s = s + "indice MF = " + indiceMeilleurFils;
            s += String.Format(" note= {0}\n", fils[indiceMeilleurFils] == null ? "?" : ((1F * fils[indiceMeilleurFils].win) / fils[indiceMeilleurFils].cross).ToString());
            int sc = 0;
            for (int k = 0; k < fils.Length; k++)
            {
                if (fils[k] != null)
                {
                    sc += fils[k].cross;
                    s += (fils[k].win + "/" + fils[k].cross + " ");
                }
                else s += (0 + "/" + 0 + " ");
            }
            s += "\n nbC=" + (sc/2);
            return s;
        }

    }

    public class JMCTS : Joueur
    {
        public static Random gen = new Random();
        static Stopwatch sw = new Stopwatch();

        float a, b;
        int temps;

        Noeud racine;

        public JMCTS(float a, float b, int temps)
        {
            this.a = 2 * a;
            this.b = 2 * b;
            this.temps = temps;
        }

        public override string ToString()
        {
            return string.Format("JMCTS[{0} - {1} - temps={2}]", a / 2, b / 2, temps);
        }

        int JeuHasard(Position p)
        {
            Position q = p.Clone();
            int re = 1;
            while (q.NbCoups > 0)
            {
                q.EffectuerCoup(gen.Next(0, q.NbCoups)); 
            }
            if (q.Eval == Resultat.j1gagne) { re = 2; }
            if (q.Eval == Resultat.j0gagne) { re = 0; }
            return re;
        }


        public override int Jouer(Position p)
        {
            sw.Restart();
            Func<int, int, float> phi = (W, C) => (a + W) / (b + C);

            racine = new Noeud(null, p);
            int iter = 0;
            while (sw.ElapsedMilliseconds < temps)
            {
                Noeud no = racine;

                do // Sélection
                {
                    no.CalculMeilleurFils(phi);
                    no = no.MeilleurFils();
                    
                } while (no.cross>0 && no.fils.Length > 0);


                int re = JeuHasard(no.p); // Simulation

                while (no != null) // Rétropropagation
                {
                    no.cross += 2;
                    no.win += re;
                    no = no.pere;
                }
                iter++;                
            }
            racine.CalculMeilleurFils(phi);
            Console.WriteLine("{0} itérations", iter);
            Console.WriteLine(racine);
            Console.WriteLine("MCTS joue {0}", racine.indiceMeilleurFils + 1);
            return racine.indiceMeilleurFils;
            
        }
    }

    public class JMCTSp : Joueur
    {
        public static Random[] gen;
        static Stopwatch sw = new Stopwatch();

        float a, b;
        int temps;
        int N;

        Noeud racine;

        public JMCTSp(float a, float b, int temps, int N)
        {
            this.a = 2 * a;
            this.b = 2 * b;
            this.temps = temps;
            this.N = N;
            gen = new Random[N];
            for(int i=0; i < N; i++) 
                gen[i] = new Random();
        }

        public override string ToString()
        {
            return string.Format("JMCTS[{0} - {1} - temps={2}]", a / 2, b / 2, temps);
        }

        int JeuHasard(Position p, int i)
        {
            Position q = p.Clone();
            int re = 1;
            while (q.NbCoups > 0)
            {
                q.EffectuerCoup(gen[i].Next(0, q.NbCoups)); 
            }
            if (q.Eval == Resultat.j1gagne) { re = 2; }
            if (q.Eval == Resultat.j0gagne) { re = 0; }
            return re;
        }


        public override int Jouer(Position p)
        {
            sw.Restart();
            Func<int, int, float> phi = (W, C) => (a + W) / (b + C);

            racine = new Noeud(null, p);
            int iter = 0;
            while (sw.ElapsedMilliseconds < temps)
            {
                Noeud no = racine;

                do // Sélection
                {
                    no.CalculMeilleurFils(phi);
                    no = no.MeilleurFils();
                    
                } while (no.cross>0 && no.fils.Length > 0);

                int re =0; 
                Task<int>[] tabTask = new Task<int>[N];
                for(int i=0; i < N; i++)
                {
                    int j=i;
                    tabTask[i] = Task.Run(() => JeuHasard(no.p, j)); // Simulation    
                }
                Task.WaitAll(tabTask);

                for(int i=0; i < N; i++)
                    re+= tabTask[i].Result;

                while (no != null) // Rétropropagation
                {
                    no.cross += 2*N;
                    no.win += re;
                    no = no.pere;
                }
                iter++;                
            }
            racine.CalculMeilleurFils(phi);
            Console.WriteLine("{0} itérations", iter);
            Console.WriteLine(racine);
            Console.WriteLine("MCTS joue {0}", racine.indiceMeilleurFils + 1);
            return racine.indiceMeilleurFils;
            
        }
    }
        public class PositionA : Position
    {
        int nb_allumettes;

        public PositionA(bool j1aletrait, int nb_allumettes):base(j1aletrait) 
        {
            this.j1aletrait = j1aletrait;
            this.nb_allumettes = nb_allumettes;
            //Initialisation des propriétés
            Eval = Resultat.indetermine; 
            NbCoups = (nb_allumettes <= 2)?nb_allumettes:3; //Cas où l'on décide de commencer à jouer avec moins de 3 al.
        }

        public override  void EffectuerCoup(int i) 
        {
            if(Eval == Resultat.indetermine) 
            {          
                nb_allumettes -= (i + 1);

                if(nb_allumettes <=0 && j1aletrait) 
                {
                    Eval = Resultat.j0gagne;
                    NbCoups = 0;
                }
                else if(nb_allumettes <=0 && !j1aletrait) 
                {
                    Eval = Resultat.j1gagne;
                    NbCoups = 0;
                }
                else
                    Eval = Resultat.indetermine;
            }
            j1aletrait = !j1aletrait;
        }

        public override Position Clone() 
        {
            PositionA clonePosition = new PositionA(j1aletrait, nb_allumettes);
            return clonePosition;
        }

        public override void Affiche()
        {
            Console.WriteLine("\n Il reste {0} allumettes au début de ce tour.", nb_allumettes);
            if(nb_allumettes!=0 )
                Console.WriteLine("C'est au joueur {0} de jouer", (j1aletrait)?1:0);
        }
    }


    public class JoueurHumainA : Joueur 
    {
        public override int Jouer(Position p) 
        {
            Console.WriteLine("Entrez un nombre de coup(s) (max = {0})",p.NbCoups);
            int saisie = int.Parse(Console.ReadLine());
            //dans le cas où le joueur décide d'entrer un nombre non autorisé
            while (saisie <= 0 || saisie > p.NbCoups) 
            {
                Console.WriteLine("Saisie non autorisée. Entrez un nombre de coup(s) (max = {0})",p.NbCoups);
                saisie = int.Parse(Console.ReadLine());
            }
            return saisie - 1;
        }
    }

}