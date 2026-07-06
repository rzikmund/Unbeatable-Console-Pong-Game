using System;
using System.Threading;

Console.CursorVisible = false;
//main variables//
int height = Console.WindowHeight;
int width = Console.WindowWidth;
int master_switch = 1;      //game ON(1) or OFF(0)
int game_speed = 55;
int win = 1;    //win for player(1), bot (2)
int timer = 0;

//border variables//
string border_symbol = "X";

//net variables//
int net_X = (int)Math.Round(width*0.50);    //net is in the "half" of the court
string net_symbol = "@";

//paddle variables//
int paddle_X = (int)(width*0.10);
int paddle_Y= height/2 - (int)(Math.Round(height * 0.10))/2;
int paddle_bottom = paddle_Y+ (int)(Math.Round(height*0.10));
string paddle_symbol = "$";

//bot paddle variables//
int bot_paddle_X = (int)(width * 0.90);
int bot_paddle_Y = height / 2 - (int)(Math.Round(height * 0.10)) / 2;
int bot_paddle_bottom = bot_paddle_Y + (int)(Math.Round(height * 0.10));

//ball variables//
int ball_X = (width-2)/2;
int ball_Y = height/2;
int ball_direction = 1; //1 = left-up; -1 = left-down; 2 = right up; -2 = right down; 0 = START position
string ball_symbol = "O";

//border//
Action border = () => {
    //left, right line//
    for (int i = 0; i < height; i++)
    {
        Console.SetCursorPosition(0, i); //left line
        Console.Write(border_symbol);
        Console.SetCursorPosition(width - 1, i);   //right line
        Console.Write(border_symbol);
    }
    //up, down line//
    for (int i = 0; i < width; i++)
    {
        Console.SetCursorPosition(i, 0);    //up line
        Console.Write(border_symbol);
        Console.SetCursorPosition(i, height - 1);  //down line
        Console.Write(border_symbol);
    }
};

//net//
Action net = () =>
{
    for (int i = 0; i < height; i++) 
    {
        Console.SetCursorPosition(net_X, i);
        Console.Write(net_symbol);
    }
};

//player paddle//
Action paddle_player = () =>
{
    for (int i = paddle_Y; i < paddle_bottom; i++)
    {
        //render//
        Console.SetCursorPosition(paddle_X, i);
        Console.Write(paddle_symbol);
    }
};


//player paddle delete//
Action paddle_player_delete = () =>
{
    for (int i = paddle_Y; i < paddle_bottom; i++)
    {
        Console.SetCursorPosition(paddle_X, i);
        Console.Write(" ");
    }
};


//bot paddle//
Action bot_paddle = () =>
{
    for (int i = bot_paddle_Y; i < bot_paddle_bottom; i++)
    {

        Console.SetCursorPosition(bot_paddle_X, i);
        Console.Write(paddle_symbol);
    }
};

//bot paddle delete//
Action bot_paddle_delete = () =>
{
    for (int i = bot_paddle_Y; i < bot_paddle_bottom; i++)
    {
        Console.SetCursorPosition(bot_paddle_X, i);
        Console.Write(" ");
    }
};

//ball collision detection//
Action collision_detection = () =>
{   
    //ball with wall//
    if (ball_Y >= height-2 && ball_direction == -1)
    {
        ball_direction = 1;
    }

    if (ball_Y <= 1 && ball_direction == 1)
    {
        ball_direction = -1;
    }

    if (ball_Y >= height - 2 && ball_direction == -2)
    {
        ball_direction = 2;
    }
    
    if (ball_Y <= 1 && ball_direction == 2)
    {
        ball_direction = -2;
    }
    
    //ball with paddle//
    if ((ball_X == paddle_X + 1 || ball_X ==paddle_X || ball_X == paddle_X-1) && ball_Y >= paddle_Y && ball_Y <= paddle_bottom && ball_direction == -1)
    {
        ball_direction = -2;
        timer = timer + 1;
    }
    
    if ((ball_X == paddle_X + 1 || ball_X == paddle_X || ball_X == paddle_X - 1) && ball_Y >= paddle_Y && ball_Y <= paddle_bottom && ball_direction == 1)
    {
        ball_direction = 2;
        timer = timer + 1;
    }
    
    //bot paddle
    if ((ball_X >= bot_paddle_X -1 && ball_Y >= bot_paddle_Y) && ball_Y <= bot_paddle_bottom && ball_direction == 2)
    {
        ball_direction = 1;
        
    }
    if ((ball_X >= bot_paddle_X + 1 && ball_Y >= bot_paddle_Y) && ball_Y <= bot_paddle_bottom && ball_direction == -2)
    {
        ball_direction = -1;
        
    }
    //end of the game//
    if (ball_X >= width - 1) //win
    {
        master_switch = 0;
        win = 1;
    }

    if (ball_X <= 1)    //lost
    {
        master_switch = 0;
        win = 2;

    }
};

//ball delete//
Action ball_delete = () =>
{
    Console.SetCursorPosition(ball_X, ball_Y);
    Console.Write(" ");

};

Action ball = () =>
{
    ball_delete();

        switch (ball_direction)
        {
            case 0: //0 = START position
                ball_X = (width - 2) / 2;
                ball_Y = height / 2;
                break;

            case 1:     //1 = left-up
                ball_X = ball_X - 1;
                ball_Y = ball_Y - 1;
                break;

            case -1:    //-1 = left-down
                ball_X = ball_X - 1;
                ball_Y = ball_Y + 1;
                break;

            case 2: //2 = right up
                ball_X = ball_X + 1;
                ball_Y = ball_Y - 1;
                break;

            case -2:    //-2 = right down
                ball_X = ball_X + 1;
                ball_Y = ball_Y + 1;
                break;
        }
        collision_detection();
        Console.SetCursorPosition(ball_X, ball_Y);
        Console.Write(ball_symbol);

   
};



//player movement//
Action player_movement = () =>
{
    
  
        if (Console.KeyAvailable)   
        {
            ConsoleKeyInfo key = Console.ReadKey(true);

        while (Console.KeyAvailable)    //delete drift from player
        {
            key = Console.ReadKey(true);
        }
        
            if (key.Key == ConsoleKey.W)    //detect if W is presed
            {   
                paddle_player_delete();
                paddle_Y = paddle_Y -1;
                paddle_bottom = paddle_bottom - 1;
                //movement_checking//
                if (paddle_bottom <= 0 || paddle_Y <= 0)
                {
                    paddle_bottom = paddle_bottom + 1;
                    paddle_Y = paddle_Y + 1;
                } 
                paddle_player();    //render
            }
            
            if (key.Key == ConsoleKey.S)    //detect if S is presed
            {
                paddle_player_delete();
                paddle_Y = paddle_Y + 1;
                paddle_bottom = paddle_bottom + 1;
                //movement_checking//
                if (paddle_bottom >= height || paddle_Y >= height)
                {
                    paddle_bottom = paddle_bottom - 1;
                    paddle_Y = paddle_Y - 1;
                }
                paddle_player();    //render
            }

        }
    
};

//bot paddle movement//
Action bot_movement = () =>
{
    
    bot_paddle_delete();
    bot_paddle_Y = ball_Y;
    bot_paddle_bottom = bot_paddle_Y + (int)(Math.Round(height * 0.10));
    
    if (bot_paddle_Y >= height- (int)(Math.Round(height * 0.10))-1)
    {
        
        bot_paddle_Y = height - (int)(Math.Round(height * 0.10))-1;
        bot_paddle_bottom = bot_paddle_Y + (int)(Math.Round(height * 0.10));

    }
    
    bot_paddle();
};

border();

//-GAME LOOP-//
do
{
    
    net();
    bot_movement();
    player_movement();
    paddle_player();    
    Thread.Sleep(game_speed);
    ball();

    switch (timer)
    {
        case 5:
            game_speed = 50;
            break;
        case 20:
            game_speed = 40;
            break;
        case 30:
            game_speed = 30;
            break;
        case 40:
            game_speed = 20;
            break;
    }
} while (master_switch==1);



//announcing the winner//
switch (win)
{
    case 1:
        Console.Clear();
        border();
        Console.SetCursorPosition(width/2,height/2);
        Console.Write(@"
                        ██╗   ██╗ ██████╗ ██╗   ██╗    ██╗    ██╗ ██████╗ ███╗   ██╗
                        ╚██╗ ██╔╝██╔═══██╗██║   ██║    ██║    ██║██╔═══██╗████╗  ██║
                         ╚████╔╝ ██║   ██║██║   ██║    ██║ █╗ ██║██║   ██║██╔██╗ ██║
                          ╚██╔╝  ██║   ██║██║   ██║    ██║███╗██║██║   ██║██║╚██╗██║
                           ██║   ╚██████╔╝╚██████╔╝    ╚███╔███╔╝╚██████╔╝██║ ╚████║
                           ╚═╝    ╚═════╝  ╚═════╝      ╚══╝╚══╝  ╚═════╝ ╚═╝  ╚═══╝
        ");

        border();
        Thread.Sleep(5000);
        break;
    
    case 2:
        Console.Clear();
        border();
        Console.SetCursorPosition(width / 2, height / 2);
        Console.Write(@"
                        ██╗   ██╗ ██████╗ ██╗   ██╗    ██╗      ██████╗ ███████╗████████╗
                        ╚██╗ ██╔╝██╔═══██╗██║   ██║    ██║     ██╔═══██╗██╔════╝╚══██╔══╝
                         ╚████╔╝ ██║   ██║██║   ██║    ██║     ██║   ██║███████╗   ██║ 
                          ╚██╔╝  ██║   ██║██║   ██║    ██║     ██║   ██║╚════██║   ██║
                           ██║   ╚██████╔╝╚██████╔╝    ███████╗╚██████╔╝███████║   ██║
                           ╚═╝    ╚═════╝  ╚═════╝     ╚══════╝ ╚═════╝ ╚══════╝   ╚═╝
                      ");
        border();
        Thread.Sleep(5000);
        break;
}
