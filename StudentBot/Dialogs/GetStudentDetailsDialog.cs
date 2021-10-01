// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio CoreBot v4.14.0

using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using System.Threading;
using System.Threading.Tasks;

namespace StudentBot.Dialogs
{
    public class GetStudentDetailsDialog : HelpDialog
    {
        private const string GetStudentDetailsMsgText = "Please enter a student name (e.g. Avisha or Ishvi).";        

        public GetStudentDetailsDialog()
            : base(nameof(GetStudentDetailsDialog))
        {
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                GetStudentNameStepAsync,
                DisplayStudentDetailsStepAsync
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> GetStudentNameStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var studentDetails = (StudentDetails)stepContext.Options;

            if (studentDetails.Name == null)
            {
                var promptMessage = MessageFactory.Text(GetStudentDetailsMsgText, GetStudentDetailsMsgText, InputHints.ExpectingInput);                
                return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
            }

            return await stepContext.NextAsync(studentDetails.Name, cancellationToken);            
        }

        private async Task<DialogTurnResult> DisplayStudentDetailsStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var studentName = (string)stepContext.Result;
            var studentDetails = new StudentDetails();
            var messageText = string.Empty;

            switch (studentName)
            {
                case "Avisha":
                    studentDetails.Name = studentName;
                    studentDetails.Class = "5A";
                    studentDetails.Teacher = "Mr. Butler";
                    messageText = "Name: " + studentDetails.Name + ", Class: " + studentDetails.Class + ", Teacher: " + studentDetails.Teacher;
                    break;

                case "Ishvi":
                    studentDetails.Name = studentName;
                    studentDetails.Class = "3A";
                    studentDetails.Teacher = "Ms. Crabb";
                    messageText = "Name: " + studentDetails.Name + ", Class: " + studentDetails.Class + ", Teacher: " + studentDetails.Teacher;
                    break;
                
                default:
                    messageText = "Sorry! Student not found in the directory!";
                    break;
            }

            await stepContext.Context.SendActivityAsync(
                    MessageFactory.Text(messageText, inputHint: InputHints.IgnoringInput), cancellationToken);

            return await stepContext.EndDialogAsync(null, cancellationToken);
        }
    }
}
