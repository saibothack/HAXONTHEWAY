using System;
using System.Collections.Generic;
using System.IO;
using HaxOnTheWay.Models;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using TouchTracking;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HaxOnTheWay.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CommandSignaturePage : ContentPage
    {
        Dictionary<long, FingerPaintPolyline> inProgressPolylines = new Dictionary<long, FingerPaintPolyline>();
        List<FingerPaintPolyline> completedPolylines = new List<FingerPaintPolyline>();

        SKPaint paint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.Blue,
            StrokeWidth = 20,
            StrokeCap = SKStrokeCap.Round,
            StrokeJoin = SKStrokeJoin.Round
        };

        public int iCommand { get; set; }
        public int iCalifica { get; set; }

        public SKImage skImage { get; set; }

        public CommandSignaturePage(int idCommand, int idCalifica)
        {
            InitializeComponent();
            iCommand = idCommand;
            iCalifica = idCalifica;
            lblNombre.IsVisible = false;
            txtNombre.IsVisible = false;
        }

       

        void OnTouchEffectAction(object sender, TouchActionEventArgs args)
        {
            switch (args.Type)
            {
                case TouchActionType.Pressed:
                    if (!inProgressPolylines.ContainsKey(args.Id))
                    {

                        FingerPaintPolyline polyline = new FingerPaintPolyline
                        {
                            StrokeColor = Color.Black,
                            StrokeWidth = 5
                        };
                        polyline.Path.MoveTo(ConvertToPixel(args.Location));

                        inProgressPolylines.Add(args.Id, polyline);
                        canvasView.InvalidateSurface();
                    }
                    break;

                case TouchActionType.Moved:
                    if (inProgressPolylines.ContainsKey(args.Id))
                    {
                        FingerPaintPolyline polyline = inProgressPolylines[args.Id];
                        polyline.Path.LineTo(ConvertToPixel(args.Location));
                        canvasView.InvalidateSurface();
                    }
                    break;

                case TouchActionType.Released:
                    if (inProgressPolylines.ContainsKey(args.Id))
                    {
                        completedPolylines.Add(inProgressPolylines[args.Id]);
                        inProgressPolylines.Remove(args.Id);
                        canvasView.InvalidateSurface();
                    }
                    break;

                case TouchActionType.Cancelled:
                    if (inProgressPolylines.ContainsKey(args.Id))
                    {
                        inProgressPolylines.Remove(args.Id);
                        canvasView.InvalidateSurface();
                    }
                    break;
            }
        }

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKCanvas canvas = args.Surface.Canvas;
            canvas.Clear();

            foreach (FingerPaintPolyline polyline in completedPolylines)
            {
                paint.Color = polyline.StrokeColor.ToSKColor();
                paint.StrokeWidth = polyline.StrokeWidth;
                canvas.DrawPath(polyline.Path, paint);
            }

            foreach (FingerPaintPolyline polyline in inProgressPolylines.Values)
            {
                paint.Color = polyline.StrokeColor.ToSKColor();
                paint.StrokeWidth = polyline.StrokeWidth;
                canvas.DrawPath(polyline.Path, paint);
            }

            skImage = args.Surface.Snapshot();
        }

        SKPoint ConvertToPixel(Point pt)
        {
            return new SKPoint((float)(canvasView.CanvasSize.Width * pt.X / canvasView.Width),
                               (float)(canvasView.CanvasSize.Height * pt.Y / canvasView.Height));
        }

        float ConvertToPixel(float fl)
        {
            return (float)(canvasView.CanvasSize.Width * fl / canvasView.Width);
        }

        private async void Hecho_Clicked(object sender, EventArgs e)
        {
            Signature sendSiganture = new Signature();
            sendSiganture.iCommand = iCommand;
            sendSiganture.iCalifica = iCalifica;
            sendSiganture.sName = txtNombre.Text;

            SKImage image = skImage;
            SKData encoded = image.Encode();
            Stream stream = encoded.AsStream();

            BinaryReader br = new BinaryReader(stream);
            sendSiganture.oImagen = Convert.ToBase64String(br.ReadBytes((int)stream.Length));
            await App.oServiceManager.SendSignture(sendSiganture);

            //Commands item = await App.Database.GetCommandAsync(iCommand);
            //await App.Database.DeleteCommandAsync(item);
            App.Database.dropTablesCommands();
            List<Drivers> drivers = await App.Database.GetAllDriversAsync();
            List<Commands> requestCommands = await App.oServiceManager.RefreshDataAsync(drivers[0]);

            foreach (Commands itm in requestCommands)
            {
                await App.Database.SaveCommandAsync(itm);
            }

            App.Current.MainPage = new NavigationPage(new CommandsPage());
            //App.Current.MainPage = new NavigationPage(new CommandSignaturePage(iCommand, iCalifica));
        }

        private void Borrar_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new CommandSignaturePage(iCommand, iCalifica));
        }
    }
}
