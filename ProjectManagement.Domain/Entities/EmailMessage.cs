using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Domain.Entities
{
    public class EmailMessage
    {
        [Required] public string To { get; set; } = string.Empty;

        [Required] public string Subject { get; set; } = string.Empty;

        [Required] public string Body { get; set; }

        public List<System.Net.Mail.Attachment> Attachments { get; set; }


        public static EmailMessage SuccessSendRequest(string email, string fullname)
        {
            return new EmailMessage()
            {
                To = email,
                Subject = "Your request has been approved",
                Body = @$"
                    <html>
                    <head>
                        <style>
                            body {{
                                font-family: Arial, sans-serif;
                                background-color: #f4f4f4;
                                margin: 0;
                                padding: 0;
                            }}
                            .container {{
                                max-width: 600px;
                                background: #ffffff;
                                margin: 20px auto;
                                padding: 20px;
                                border-radius: 8px;
                                box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
                            }}
                            h2 {{
                                color: #333;
                            }}
                            p {{
                                font-size: 16px;
                                color: #555;
                                line-height: 1.5;
                            }}
                            .footer {{
                                margin-top: 20px;
                                font-size: 14px;
                                color: #888;
                                text-align: center;
                            }}
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <h2>Your request has been approved!</h2>
                            <p>Dear, {fullname}</p>
                            <p>We are pleased to inform you that your request has been successfully approved and is now being processed. Our specialists will contact you soon.</p>
                            <p>Thank you for choosing our service!</p>
                            <div class='footer'>
                                <p>&copy; {DateTime.UtcNow.Year} CRM System | All rights reserved</p>
                            </div>
                        </div>
                    </body>
                    </html>
                "
            };
        }


        public static EmailMessage DenySendRequest(string email, string fullname)
        {
            return new EmailMessage()
            {
                To = email,
                Subject = "Your request has been rejected",
                Body = @$"
                    <html>
                    <head>
                        <style>
                            body {{
                                font-family: Arial, sans-serif;
                                background-color: #f4f4f4;
                                margin: 0;
                                padding: 0;
                            }}
                            .container {{
                                max-width: 600px;
                                background: #ffffff;
                                margin: 20px auto;
                                padding: 20px;
                                border-radius: 8px;
                                box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
                            }}
                            h2 {{
                                color: #333;
                            }}
                            p {{
                                font-size: 16px;
                                color: #555;
                                line-height: 1.5;
                            }}
                            .footer {{
                                margin-top: 20px;
                                font-size: 14px;
                                color: #888;
                                text-align: center;
                            }}
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <h2>Your request has been rejected</h2>
                            <p>Dear, {fullname}</p>
                            <p>Unfortunately, your request has been rejected. If you have any questions, please contact our support team for clarification.</p>
                            <p>We apologize for the inconvenience.</p>
                            <div class='footer'>
                                <p>&copy; {DateTime.UtcNow.Year} CRM System | All rights reserved</p>
                            </div>
                        </div>
                    </body>
                    </html>
                "
            };
        }

        public static EmailMessage ForAddNewUser(string email, string fullname, string position, string temp_password)
        {
            return new EmailMessage()
            {
                To = email,
                Subject = "Welcome to the CRM",
                Body = @$"
                    <html>
                    <head>
                        <style>
                            body {{
                                font-family: Arial, sans-serif;
                                background-color: #f4f4f4;
                                margin: 0;
                                padding: 0;
                            }}
                            .container {{
                                max-width: 600px;
                                background: #ffffff;
                                margin: 20px auto;
                                padding: 20px;
                                border-radius: 8px;
                                box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
                            }}
                            h2 {{
                                color: #333;
                            }}
                            p {{
                                font-size: 16px;
                                color: #555;
                                line-height: 1.5;
                            }}
                            .footer {{
                                margin-top: 20px;
                                font-size: 14px;
                                color: #888;
                                text-align: center;
                            }}
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <h2>Welcome to the CRM, {fullname}!</h2>
                            <p>Hello {fullname},</p>
                            <p>We are excited to welcome you to our team as a <strong>{position}</strong>!</p>
                            <p>Your journey with us starts now, and we can't wait to see the impact you'll make. Below are your login details to get started:</p>
                            <p><strong>Username:</strong> {email}</p>
                            <p><strong>Temporary Password:</strong> {temp_password}</p>
                            <p>Please log in and change your password as soon as possible.</p>
                            <p>If you have any questions, feel free to reach out. We’re happy to have you on board!</p>
                            <p>Best regards,</p>
                            <div class='footer'>
                                <p>&copy; {DateTime.UtcNow.Year} CRM System | All rights reserved</p>
                            </div>
                        </div>
                    </body>
                    </html>
                "
            };
        }


    }
}
