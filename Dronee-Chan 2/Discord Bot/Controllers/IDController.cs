using Dronee_Chan_2.Discord_Bot.Events;
using Dronee_Chan_2.Discord_Bot.Objects.UserObjects;
using DSharpPlus.Entities;
using System.Drawing.Imaging;
using System.Drawing;
using Dronee_Chan_2.Discord_Bot.Objects.ControllerObjects;
using System.Net;

namespace Dronee_Chan_2.Discord_Bot.Controllers
{
    internal class IDController
    {


        DiscordGuild DiscordGuild { get; set; }
        ImageList imageList = new ImageList();


        public IDController(DiscordGuild discordGuild)
        {
            DiscordGuild = discordGuild;
            EventManager.GenerateIDCEventRaised += EventManager_GenerateIDCEventRaised;
        }
        private Image getRankImage(int rank)
        {
            //if (rank == 101)
            //    return Image.FromFile(ImageList.ranks[20]);

            int fileIndex = ((int)rank - 1) / 5;
            int slot = ((int)rank - 1) % 5 + 1;

            //Console.WriteLine((rank % 5) + " - " + ((int)(rank % 5)) + " - " + ((int)(rank % 5)) / 2);

            return cropRankImage(Image.FromFile(imageList.ranks[fileIndex]), slot);
        }

        private Image cropRankImage(Image image, int slot)
        {
            Bitmap bmpImage = new Bitmap(image);
            return bmpImage.Clone(new Rectangle(new Point(0 + ((slot - 1) * 66), 0), new Size(66, 53)), bmpImage.PixelFormat);
        }
        public static string MakeValidFileName(string name)
        {
            string invalidChars = System.Text.RegularExpressions.Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars()));
            string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

            return System.Text.RegularExpressions.Regex.Replace(name, invalidRegStr, "_");
        }

        private async Task getAvatar(DiscordMember user)
        {
            if (File.Exists($"C:\\Images\\{MakeValidFileName(getUsername(user).Trim())}.png"))
                return;

            using (WebClient webClient = new WebClient())
            {
                string URI = user.AvatarUrl;
                if (URI == null)
                {
                    File.Copy(@"C:\Images\noimage.png", $"C:\\Images\\{getUsername(user).Trim()}.png");
                    return;
                }
                string FilePath = $"C:\\Images\\{MakeValidFileName(getUsername(user).Trim())}.png";
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                await Task.Run(() => webClient.DownloadFile(new Uri(URI), FilePath));

                webClient.Dispose();
            }

        }

        private string getUsername(DiscordMember user)
        {
            string nickname = user.Nickname;
            if (nickname != null)
            {
                if (nickname.Contains('|'))
                {
                    return nickname.Split('|')[1];
                }
                return nickname;
            }
            return user.Username;
        }

        private async Task<string> EventManager_GenerateIDCEventRaised(User user)
        {
            Image img = mergeImages(Image.FromFile(imageList.background), Image.FromFile(imageList.logo), 0, 0);

            if (user.Infected)
                img = mergeImages(img, Image.FromFile(imageList.infected), 0, 0);


            double rank = await EventManager.CalculateRank(user);

            img = mergeImages(img, getRankImage((int)rank),472,524);

            int barNr = (int)((rank - ((int)rank)) * 10);
            if (barNr != 0)
                img = mergeImages(img, Image.FromFile("C:\\Images\\" + barNr + " Bar.png"), 0, 0);

            DiscordMember member = await DiscordGuild.GetMemberAsync(user.DiscordUUID);
            await getAvatar(member);
            img = mergeImages(img, ScaleImage(Image.FromFile($"C:\\Images\\{MakeValidFileName(getUsername(member))}.png"),128,128), 74, 147);

            img = mergeImages(img, Image.FromFile(imageList.avatarFrame), 0, 0);
            img = mergeImages(img, Image.FromFile(imageList.awardFrame), 0, 0);
            img = mergeImages(img, Image.FromFile(imageList.ribbonsBackground), 0, 0);
            img = mergeImages(img, Image.FromFile(imageList.XPBar), 0, 0);
            if (user.IsVIP)
                img = mergeImages(img, Image.FromFile(imageList.VIP), 0, 0);

            //inventory
            Point p = new Point(715, 465);
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    img = mergeImages(img, Image.FromFile(imageList.InventoryBlank), p.X, p.Y);
                    p.X += 47;
                }
                p.X = 715;
                p.Y += 47;
            }


            p = new Point(715, 465);
            foreach (string s in user.Inventory)
            {
                if (p.X >= 949)
                {
                    p.X = 715;
                    p.Y += 47;
                }
                Image icon;
                Item item = await EventManager.GetItemByID(int.Parse(s));
                if (File.Exists(imageList.InventoryPath + item.Icon + ".png"))
                    icon = Image.FromFile(imageList.InventoryPath + item.Icon + ".png");
                else
                    icon = Image.FromFile(imageList.InventoryUnknown);
                img = mergeImages(img, icon, p.X, p.Y);
                p.X += 47;
            }

            //Streak
            int streak = user.StreakCounter;
            p = new Point(960, 465);
            for (int i = 0; i < 5; i++)
            {
                if (streak <= 0)
                    break;
                for (int j = 0; j < 2; j++)
                {
                    img = mergeImages(img, Image.FromFile(imageList.Streak), p.X, p.Y);
                    p.X += 21;
                    streak--;
                    if (streak <= 0)
                        break;
                }
                p.X = 960;
                p.Y += 27;
            }

            StringFormat format = new StringFormat();
            img = addTextToImage(img, getCallsign(member) + "\n" + getUsername(member) + "\n" + FindRank(member.Roles), new Font("AmarilloUSAF", 25), new SolidBrush(Color.FromArgb(127, 0, 0)), new PointF(10f, 500f), format);
            img = addTextToImage(img, "Last seen: " + user.LastSeen.ToShortDateString(), new Font("AmarilloUSAF", 15), new SolidBrush(Color.FromArgb(127, 0, 0/*13, 14, 15*/)), new PointF(535f, 440f), format);
            img = addTextToImage(img, "Currency: R$" + user.Currency + "", new Font("AmarilloUSAF", 15), new SolidBrush(Color.FromArgb(127, 0, 0)), new PointF(15f, 440f), format);
            img = addTextToImage(img, "Flights: " + user.FlightsAmount + "", new Font("AmarilloUSAF", 15), new SolidBrush(Color.FromArgb(127, 0, 0)), new PointF(890f, 440f), format);
            img = addTextToImage(img, "Join: " + member.JoinedAt.DateTime.ToShortDateString() + "", new Font("AmarilloUSAF", 15), new SolidBrush(Color.FromArgb(127, 0, 0)), new PointF(740f, 440f), format);
            img = addTextToImage(img, "Clearance Level: " + getClearanceLevel(member.Roles) + "", new Font("AmarilloUSAF", 15), new SolidBrush(Color.FromArgb(127, 0, 0)), new PointF(15f, 11f), format);
            img = addTextToImage(img, user.DiscordUUID + "", new Font("AmarilloUSAF", 15), new SolidBrush(Color.FromArgb(127, 0, 0)), new PointF(520f, 11f), format);
            format.Alignment = StringAlignment.Center;
            img = addTextToImage(img, findRankName((int)rank), new Font("AmarilloUSAF", 20), new SolidBrush(Color.FromArgb(127, 0, 0)), new PointF(505f, 490f), format);


            img.Save(imageList.Result, ImageFormat.Png);
            return imageList.Result;
        }
        private string findRankName(int rank)
        {
            if (rank > 100)
                return "[" + rank + "]Myth";
            if (rank > 90)
                return "[" + rank + "]Legend-" + (rank - 90);
            if (rank > 80)
                return "[" + rank + "]Elite-" + (rank - 80);
            if (rank > 70)
                return "[" + rank + "]Warlord-" + (rank - 70);
            if (rank > 60)
                return "[" + rank + "]Ace-" + (rank - 60);
            if (rank > 50)
                return "[" + rank + "]Veteran-" + (rank - 50);
            if (rank > 40)
                return "[" + rank + "]Professional-" + (rank - 40);
            if (rank > 30)
                return "[" + rank + "]Mercenary-" + (rank - 30);
            if (rank > 20)
                return "[" + rank + "]Enlisted-" + (rank - 20);
            if (rank > 10)
                return "[" + rank + "]Recruit-" + (rank - 10);
            if (rank > 0)
                return "[" + rank + "]Civilian-" + (rank);
            return "Unknown Rank";
        }
        private int getClearanceLevel(IEnumerable<DiscordRole> roles)
        {
            if (roles.Any(r => r.Id == 832196351126798336)) //Staff+
                return 4;
            if (roles.Any(r => r.Id == 964276908156145734)) //Staff
                return 3;
            if (roles.Any(r => r.Id == 1027941538027798558)) //Raven
                return 2;
            if (roles.Any(r => r.Id == 822286304657932289)) //Member
                return 1;
            return 0;
        }
        private string FindRank(IEnumerable<DiscordRole> roles)
        {
            string Rank = "";
            

            if (roles.Any(r => r.Id == 1027942259305484359))
            {
                Rank += "13th Rooks";
            } else if (roles.Any(r => r.Id == 1027941538027798558))
            {
                Rank += "32nd Ravens";
            } else if (roles.Any(r => r.Id == 964238880389865552))
            {
                Rank += "51st RRF";
            } else if (roles.Any(r => r.Id == 822286304657932289))
            {
                Rank += "Contractor";
            } else
            {
                Rank += "No Rank";
            }

            if (roles.Any(r => r.Id == 1027989330574184559))
                Rank += " - Wing Lead";
            return Rank;
        }
        private string getCallsign(DiscordMember user)
        {
            if (System.String.IsNullOrEmpty(user.Nickname))
            {
                return "No Callsign";
            }
            string[] s = user.Nickname.Split('|');
            if (s.Length <= 1)
                return "No Callsign";
            return s[0];
        }
        private Image addTextToImage(Image imageBackground, string text, Font font, Brush brush, PointF p, StringFormat format)
        {
            Graphics.FromImage(imageBackground).DrawString(text, font, brush, p.X, p.Y, format);

            return imageBackground;
        }
        private Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            if(image.Width <= maxWidth && image.Height <= maxHeight)
                return image;

            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
            {
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);
            }

            return newImage;
        }

        private Image mergeImages(Image imageBackground, Image imageOverlay, int x, int y)
        {
            Image img = new Bitmap(imageBackground.Width, imageBackground.Height);
            using (Graphics gr = Graphics.FromImage(img))
            {
                ((Bitmap)imageBackground).SetResolution(gr.DpiX, gr.DpiY);
                ((Bitmap)imageOverlay).SetResolution(gr.DpiX, gr.DpiY);
                gr.DrawImage(imageBackground, new Point(0, 0));
                gr.DrawImage(imageOverlay, new Point(x, y));
            }
            imageBackground.Dispose();
            imageOverlay.Dispose();
            return img;
        }
    }
}
