using System;
using System.Collections.Generic;
using System.Threading;
using TreasureHunter.Interfaces;
using TreasureHunter.Models;

namespace TreasureHunter
{
  public class App : IApp
  {
    public IPlayer Player { get; set; }
    public IPlayer CoWorker { get; set; }
    public IBoundary Location { get; set; }

    private bool playing = true;
    public bool Playing { get { return playing; } set { playing = value; } }
    public void Setup()
    {
      Boundary lobby = new Boundary("Lobby", " You find yourself in the lobby, always on the lookout for HR and dodging darts as your coworkes shoot at you");
      Boundary mainOffice = new Boundary("Office", "You are around the office running around as you dodge the darts that are being shot at you. Meanwhile the day keeps on going. Remember HR is always lurking");
      Boundary myDesk = new Boundary("Desk", "You come to your cubible thinking that it may be a safe space, but oh so wrong you were. You gotta keep shooting");
      Boundary breakRoom = new Boundary("Cafe", "Even in the Cafe you aren't safe. You got to keep shooting");

      lobby.AddNeighborBoundary(mainOffice, true);
      myDesk.AddNeighborBoundary(breakRoom, true);
      mainOffice.AddNeighborBoundary(myDesk, true);
      lobby.AddNeighborBoundary(myDesk, true);
      lobby.AddNeighborBoundary(breakRoom, true);
      mainOffice.AddNeighborBoundary(breakRoom, true);

      Location = lobby;

      Item gun = new Item("Cricket", "One dart");
      Item ammo = new Item("Dart", "For Cricket use");
      gun.AmmoCount = 1;
      myDesk.Items.Add(ammo);
      mainOffice.Items.Add(ammo);
      breakRoom.Items.Add(ammo);
      lobby.Items.Add(ammo);
      // lobby.Items[0].AmmoCount = 0;
      // myDesk.Items[0].AmmoCount = 5;
      // mainOffice.Items[0].AmmoCount = 0;
      // breakRoom.Items[0].AmmoCount = 0;

      Player = new Player("Joe");
      Player.Inventory.Add(gun);
      Player.Inventory.Add(ammo);
      // Player.Inventory[1].AmmoCount = 5;
      CoWorker = new CoWorker(Team());

    }

    public string Team()
    {
      Random rndGender = new Random();
      Random rndName = new Random();

      int gender = rndGender.Next(0, 1);
      int name = rndName.Next(0, 9);

      string[] maleNames = new string[] { "Aaron", "Roger", "Mike", "Nathan", "Mark", "Will", "Kevin", "Bob", "Dustin", "Daniel" };
      string[] femaleNames = new string[] { "Jen", "Amy", "Kelly", "Julie", "Kira", "Amanda", "Jessica", "Linda", "Sarah", "Tiffany" };

      if (gender == 0)
      {
        return maleNames[name];
      }
      else
      {
        return femaleNames[name];
      }
    }
    public void Run()
    {
      Console.ResetColor();
      Greeting();
      while (Playing)
      {
        DisplayMenu();
        CaptureUserInput();
      }
    }
    public void Greeting()
    {
      foreach (var ltr in "Welcome to...")
      {
        System.Console.Write(ltr);
        Thread.Sleep(200);
      }
      Console.Clear();
      GreetingPic();
      Thread.Sleep(1200);
      Console.Clear();
      System.Console.WriteLine($@"
      
      Good morning! you’re at another day of work. Before you decide to go about your day. You remember there's an ougoing nerf gun war at the office. 
      This is a war between you and your coworkers that has been going on for a while, keeping in mind as soon as you start, you have to start shooting 
      your coworkers. You’re to decide whether you want keep grinding through the day, or just turn leave for the day and go home.

      Keeping in mind that no matter where you are, you're never safe. You have a nerf gun called 'Cricket' with you at all times, and you'll find yourself 
      with some darts to begin with. Your coworkers will be shooting all the time, as you move or shoot they may or may not hit you. These darts will fall 
      on the ground and you can retreive them,so you can increase your inventory. 
      
      You also have to keep in mind that HR is always lurking around and may or may not be in the room with you. If they happen to be in the room with you, 
      it is encouraged that you move to a different room, or if you want to risk it. You can shoot your coworkers. Doing this though, will increase your 
      chances of losing. If they catch you constantly doing this, they'll eventually take your gun away and you'll automatically lose the game. 

      The game ends when the work day ends and whoever has the most points between you and one of your coworkers. 
");

    }
    public void DisplayMenu()
    {
      System.Console.WriteLine("Menu:\n\n");
      System.Console.WriteLine("You may roam around the work space. You may go between: Lobby, Desk, Cafe, or Office\n");
      System.Console.WriteLine("You're welcome to (shoot) your coworkers, (Look) for some darts, or (Quit) if you want to go home instead.\n");
      System.Console.WriteLine("Type 'Help' - For a list of all available commands.\n");
    }
    public void CaptureUserInput()
    {

      System.Console.WriteLine("What would you like to do?\n");

      string userInput = Console.ReadLine().ToLower();
      string[] words = userInput.Split(' ');
      string command = words[0];
      string option = "";
      if (words.Length > 1)
      {
        option = words[1];
      }
      switch (command)
      {
        case "help":
          DisplayHelpInfo();
          Around();
          break;
        case "inventory":
          DisplayPlayerInventory();
          break;
        case "look":
          DisplayRoomDescription();
          break;
        case "take":
          TakeItem(option);
          break;
        case "shoot":
          Shoot();
          CleanUp();
          break;
        case "go":
          ChangeLocation(option);
          CleanUp();
          break;
        case "score":
          CurrentScore();
          break;
        case "quit":
          System.Console.WriteLine("You have decided to leave work for the day.\n\n");
          Playing = false;
          break;
        default:
          System.Console.WriteLine("Not a valid option\n");
          break;
      }
    }

    public void CleanUp()
    {
      HRpresent();
      MyMorale();
      TeamShooting();
      UpdateMovesRemaining();
    }

    private void Around()
    {
      System.Console.WriteLine(Location.Description);
    }

    private void MyMorale()
    {
      if (Player.Morale <= 0)
      {
        Playing = false;
        Console.ForegroundColor = ConsoleColor.DarkRed;
        System.Console.WriteLine("HR took your gun away. You Lose!");
        Console.ResetColor();
      }
    }

    private void TeamShooting()
    {

      Random rndShoot = new Random();
      int shooting = rndShoot.Next(1, 10);
      if (shooting <= 2)
      {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        System.Console.WriteLine($"You've been shot by {Player.Name}!");
        Console.ResetColor();
        CoWorker.Score++;
      }
      Location.Items[0].AmmoCount++;
    }

    private void HRpresent()
    {
      Random rndHR = new Random();
      int hrAround = rndHR.Next(1, 10);
      if (hrAround >= 8)
      {
        Location.HRinRoom = true;
        Console.ForegroundColor = ConsoleColor.Yellow;
        System.Console.WriteLine("HR walked in the room.");
        Console.ResetColor();
      }
    }
    private void UpdateMovesRemaining()
    {
      Player.MovesRemaining--;
      if (Player.MovesRemaining == 0)
      {
        if (Player.Score > CoWorker.Score)
        {
          System.Console.WriteLine("You Win!\n");
          System.Console.WriteLine("You Win with a score of: \t" + Player.Score);
          System.Console.WriteLine("Your coworker got a score of: \t" + CoWorker.Score);
          Playing = false;
        }
        else
        {
          System.Console.WriteLine("You Lose :(\n");
          System.Console.WriteLine("You lose with a score of: \t" + Player.Score);
          System.Console.WriteLine("Coworker wins with a score of: \t" + CoWorker.Score);
          Playing = false;
        }
      }
    }

    public void CurrentScore()
    {
      System.Console.WriteLine("Current Score Points: \t" + Player.Score);
      System.Console.WriteLine("Current Morale Points: \t" + Player.Morale);
      System.Console.WriteLine("");
      System.Console.WriteLine("Press Any Key to Continue.");
      Console.ReadKey(true);
      Console.Clear();
      Around();
      CaptureUserInput();
    }
    public void DisplayRoomDescription()
    {
      if (Location.Items[0].AmmoCount > 0)
      {
        System.Console.WriteLine($"You see the following:\r\n {Location.Items[0].Name}: {Location.Items[0].AmmoCount} in the room\n");
      }
      else
      {
        System.Console.WriteLine("There are no more darts around.\n");
      }
      System.Console.WriteLine("Press Any Key to Continue.");
      Console.ReadKey(true);
      Console.Clear();
      Around();
      DisplayMenu();
      CaptureUserInput();
    }

    public void ChangeLocation(string locationName)
    {
      bool found = false;
      foreach (var key in Location.NeighborBoundaries)
      {
        if (key.Value.Name.ToLower() == locationName.ToLower())
        {
          Location = key.Value;
          found = true;
        }
      }
      if (found == false)
      {
        System.Console.WriteLine("Invalid Location.");
      }
    }
    public void TakeItem(string itemName)
    {
      Player.Inventory[1].AmmoCount = Player.Inventory[1].AmmoCount + Location.Items[0].AmmoCount;
      if (Location.Items[0].AmmoCount == 1)
      {
        System.Console.WriteLine($"You picked up {Location.Items[0].AmmoCount} dart.\n");
        Location.Items[0].AmmoCount = 0;
      }
      else if (Location.Items[0].AmmoCount > 1)
      {
        System.Console.WriteLine($"You picked up {Location.Items[0].AmmoCount} darts.\n");
        Location.Items[0].AmmoCount = 0;
      }
      else if (Location.Items[0].AmmoCount == 0)
      {
        System.Console.WriteLine("You picked up all the darts.\n");
      }
    }

    public void Shoot()
    {
      TakeShot();
    }

    public void TakeShot()
    {
      if (Player.Inventory[1].AmmoCount > 0)
      {

        Random rnd = new Random();
        int toHit = rnd.Next(0, 10);
        if (toHit >= 4)
        {
          Player.Score++;
          Console.ForegroundColor = ConsoleColor.Blue;
          System.Console.WriteLine("Nice shot\n");
          Console.ResetColor();

        }
        else if (Player.Inventory[1].AmmoCount == 0)
        {
          Console.ForegroundColor = ConsoleColor.DarkCyan;
          System.Console.WriteLine("You got no darts!\n");
          Console.ResetColor();
        }
        else
        {
          System.Console.WriteLine("You missed\n");
        }
      }
      Player.Inventory[1].AmmoCount--;
      Location.Items[0].AmmoCount++;
      if (Location.HRinRoom)
      {
        Player.Morale = Player.Morale - 2;
        Console.ForegroundColor = ConsoleColor.Red;
        System.Console.WriteLine("HR caught you shooting your coworker!\n");
        Console.ResetColor();
      }


    }
    public void DisplayPlayerInventory()
    {
      Console.Clear();
      Around();
      DisplayMenu();
      foreach (var item in Player.Inventory)
      {
        Console.ForegroundColor = ConsoleColor.Blue;
        System.Console.WriteLine($"You Currently have: {item.Name} {item.AmmoCount}\r\n");
        Console.ResetColor();
      }
      System.Console.WriteLine("Press Any Key to Continue.");
      Console.ReadKey(true);
      Console.Clear();
      Around();
      DisplayMenu();
      CaptureUserInput();
    }

    public void DisplayHelpInfo()
    {
      Console.Clear();
      Around();
      System.Console.WriteLine();
      Console.ForegroundColor = ConsoleColor.DarkCyan;
      System.Console.WriteLine("Available Commands:\n\n");
      Console.ResetColor();
      Console.ForegroundColor = ConsoleColor.Gray;
      System.Console.WriteLine("'Go <location>' - Moves you to specifc location \n");
      System.Console.WriteLine("'Look' - Searches the Room for items\n");
      System.Console.WriteLine("'Take <item>' - Takes item from room\n");
      System.Console.WriteLine("'Score' - Shows current score and morale\n");
      System.Console.WriteLine("'Inventory' - Displays Player Inventory\n");
      System.Console.WriteLine("'Quit' - ends game, you lose\n\n");
      System.Console.WriteLine("Press Any Key to Continue.");
      Console.ResetColor();
      Console.ReadKey(true);
      Console.Clear();
      Around();
      DisplayMenu();
      CaptureUserInput();
    }

    void GreetingPic()
    {
      System.Console.WriteLine(@"                                                                                                            
                                                                                                            
     OOOOOOOOO        ffffffffffffffff    ffffffffffffffff    iiii                                          
   OO:::::::::OO     f::::::::::::::::f  f::::::::::::::::f  i::::i                                         
 OO:::::::::::::OO  f::::::::::::::::::ff::::::::::::::::::f  iiii                                          
O:::::::OOO:::::::O f::::::fffffff:::::ff::::::fffffff:::::f                                                
O::::::O   O::::::O f:::::f       fffffff:::::f       ffffffiiiiiii     cccccccccccccccc    eeeeeeeeeeee    
O:::::O     O:::::O f:::::f             f:::::f             i:::::i   cc:::::::::::::::c  ee::::::::::::ee  
O:::::O     O:::::Of:::::::ffffff      f:::::::ffffff        i::::i  c:::::::::::::::::c e::::::eeeee:::::ee
O:::::O     O:::::Of::::::::::::f      f::::::::::::f        i::::i c:::::::cccccc:::::ce::::::e     e:::::e
O:::::O     O:::::Of::::::::::::f      f::::::::::::f        i::::i c::::::c     ccccccce:::::::eeeee::::::e
O:::::O     O:::::Of:::::::ffffff      f:::::::ffffff        i::::i c:::::c             e:::::::::::::::::e 
O:::::O     O:::::O f:::::f             f:::::f              i::::i c:::::c             e::::::eeeeeeeeeee  
O::::::O   O::::::O f:::::f             f:::::f              i::::i c::::::c     ccccccce:::::::e           
O:::::::OOO:::::::Of:::::::f           f:::::::f            i::::::ic:::::::cccccc:::::ce::::::::e          
 OO:::::::::::::OO f:::::::f           f:::::::f            i::::::i c:::::::::::::::::c e::::::::eeeeeeee  
   OO:::::::::OO   f:::::::f           f:::::::f            i::::::i  cc:::::::::::::::c  ee:::::::::::::e  
     OOOOOOOOO     fffffffff           fffffffff            iiiiiiii    cccccccccccccccc    eeeeeeeeeeeeee  
                                                                                                            
    
     WWWWWWWW                           WWWWWWWW                                                            
     W::::::W                           W::::::W                                                            
     W::::::W                           W::::::W                                                            
     W::::::W                           W::::::W                                                            
      W:::::W           WWWWW           W:::::Waaaaaaaaaaaaa  rrrrr   rrrrrrrrr       ssssssssss            
       W:::::W         W:::::W         W:::::W a::::::::::::a r::::rrr:::::::::r    ss::::::::::s           
        W:::::W       W:::::::W       W:::::W  aaaaaaaaa:::::ar:::::::::::::::::r ss:::::::::::::s          
         W:::::W     W:::::::::W     W:::::W            a::::arr::::::rrrrr::::::rs::::::ssss:::::s         
          W:::::W   W:::::W:::::W   W:::::W      aaaaaaa:::::a r:::::r     r:::::r s:::::s  ssssss          
           W:::::W W:::::W W:::::W W:::::W     aa::::::::::::a r:::::r     rrrrrrr   s::::::s               
            W:::::W:::::W   W:::::W:::::W     a::::aaaa::::::a r:::::r                  s::::::s            
             W:::::::::W     W:::::::::W     a::::a    a:::::a r:::::r            ssssss   s:::::s          
              W:::::::W       W:::::::W      a::::a    a:::::a r:::::r            s:::::ssss::::::s         
               W:::::W         W:::::W       a:::::aaaa::::::a r:::::r            s::::::::::::::s          
                W:::W           W:::W         a::::::::::aa:::ar:::::r             s:::::::::::ss           
                 WWW             WWW           aaaaaaaaaa  aaaarrrrrrr              sssssssssss             
                                                                                                        
");
    }


  }
}