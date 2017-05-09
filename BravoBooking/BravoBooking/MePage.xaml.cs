﻿using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using ADALForForms.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;

namespace BravoBooking
{
	public partial class MePage : ContentPage
	{
        public MePage()
        {
            InitializeComponent();
            this.Appearing += MePageAppearing;
            
            NumberOfPersonsPicker.Items.Add("4 personer");
            NumberOfPersonsPicker.Items.Add("6 personer");
            NumberOfPersonsPicker.Items.Add("10 personer");
            
            StartTimePicker.Items.Add("Akkurat nå");
            StartTimePicker.Items.Add("Neste halvtime");
            StartTimePicker.Items.Add("Neste hele time");
           
            DurationPicker.Items.Add("1 time");
            DurationPicker.Items.Add("2 timer");
            DurationPicker.Items.Add("3 timer");

            var button = new Button { Text = "Book NÅ!", TextColor = Color.FromHex("#77d065"), FontSize = 20};
            
            
        }

        private async void MePageAppearing(object sender, EventArgs e)
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.AuthenticationResult.AccessToken);
            var meData = await client.GetStringAsync("https://graph.microsoft.com/v1.0/me");
            var userData = JsonConvert.DeserializeObject<UserModel>(meData);
            
        }


        private void AntallPersoner_OnSelectedIndexChanged(Object sender, EventArgs e)
        {
            var name = NumberOfPersonsPicker.Items[NumberOfPersonsPicker.SelectedIndex];
            
        }

        private void WhenPicker_OnSelectedIndexChanged(Object sender, EventArgs e)
        {
            var name = StartTimePicker.Items[StartTimePicker.SelectedIndex];
            
        }

        private void TidPicker_OnSelectedIndexChanged(Object sender, EventArgs e)
        {
            var name = DurationPicker.Items[DurationPicker.SelectedIndex];
            
        }

        private async void BookNow_OnClicked(Object sender, EventArgs e)
        {

           
            string text = MainEntry.Text;
            string antalls=NumberOfPersonsPicker.Items[Math.Max(0,NumberOfPersonsPicker.SelectedIndex)];
            int startint = Math.Max(0,StartTimePicker.SelectedIndex);
            string varig = DurationPicker.Items[Math.Max(0,DurationPicker.SelectedIndex)];
            DateTime now = DateTime.Now;


            int antall = int.Parse(antalls[0].ToString());

            if (startint == 1)
            {
                startint = 30;
            }
            else if (startint == 2)
            {
                startint = 60;
            }
            
            DateTime date=now.AddMinutes((double)startint);

            DateTime end = date.AddHours(Double.Parse(varig[0].ToString()));;
            
            

            var client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.AuthenticationResult.AccessToken);
            var meData = await client.GetStringAsync("https://graph.microsoft.com/v1.0/users?$filter=startswith(displayName,'M')");
            var data = JsonConvert.DeserializeObject<RomModel>(meData);
            var users = from user in data.value
                        select user;
            RomModel.value2[] a = users.ToArray();
            bool done = false;

            for(int i=0; i<1;i++)
            {
                //<TODO: Kode å filtrere etter antall personer>
                bool free=true;
                //var romData = await client.GetStringAsync("https://graph.microsoft.com/v1.0/users/"+a[i].Id+ "/events");
                var romData = await client.GetStringAsync("https://graph.microsoft.com/v1.0/me/events?$select=subject,start,end");
                var dat = JsonConvert.DeserializeObject<CalendarModel>(romData);
                var events = from Event in dat.value
                             select Event;
                CalendarModel.value2[] b = events.ToArray();


                for(int j = 0; j < b.Length; j++)
                {
                    DateTime starten = Convert.ToDateTime(b[j].Start.DateTime);
                    DateTime slutten = Convert.ToDateTime(b[j].End.DateTime);
                    if ((starten > date && starten < end)||(slutten>date && slutten<end))
                    {
                        free = false;
                        break;
                    }
                }
                if (free)
                {
                    //<TODO: Sette Opp møte mellom (now) og (end) på møterom b[i]>

                    CalendarModel.value2 meeting = new CalendarModel.value2("Meeting at " + a[i].DisplayName, date.ToString(), end.ToString(), a[i].Mail, a[i].DisplayName);
                    //meeting.Start.DateTime = date.ToString("MM'/'dd'/'yyyy HH':'mm':'ss.fff");
                    //meeting.End.DateTime = end.ToString();
                    //meeting.Atendee.name = a[i].DisplayName;
                    //meeting.Atendee.email = a[i].Mail;
                    //meeting.subject = "Meeting at " + a[i].DisplayName;
                    string json = JsonConvert.SerializeObject(meeting);
                    
                    var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var httpResponse = await httpClient.PostAsync("https://graph.microsoft.com/v1.0/me/events", httpContent);
                        if (httpResponse.Content != null)
                        {
                            var send = await httpResponse.Content.ReadAsStringAsync();
                            MainLabel.Text = send.ToString();
                        }
                    }


                        //var send = await client.PostAsync("https://graph.microsoft.com/v1.0/me/events", httpContent);
                    
                    
                    done = true;
                    
                    break;
              
                }

               
                

            }
            if (done)
            {
                MainLabel.Text = "Rommet er booket";
            }
            else
            {
                MainLabel.Text = "Det er desverre ingen ledige rom som passer dine kriterier.";
            }
        }

        


    }
}

