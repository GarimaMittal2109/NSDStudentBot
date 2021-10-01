using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using System.Threading;
using System.Threading.Tasks;

namespace StudentBot.Dialogs
{
    public class CheckStudentGradeDialog : HelpDialog
    {
        private const string GetStudentDetailsMsgText = "Please enter a student name (e.g. Avisha or Ishvi).";

        public CheckStudentGradeDialog()
            : base(nameof(CheckStudentGradeDialog))
        {
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                GetStudentNameStepAsync,
                DisplayStudentGradeStepAsync
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

        private async Task<DialogTurnResult> DisplayStudentGradeStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var studentName = (string)stepContext.Result;
            var studentDetails = new StudentDetails();
            var messageText = string.Empty;

            switch (studentName)
            {
                case "Avisha":
                    studentDetails.Name = studentName;
                    studentDetails.Grade = "A+";                    
                    messageText = "Name: " + studentDetails.Name + ", Grade: " + studentDetails.Grade;
                    break;

                case "Ishvi":
                    studentDetails.Name = studentName;       
                    studentDetails.Grade = "A";
                    messageText = "Name: " + studentDetails.Name + ", Grade: " + studentDetails.Grade;
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
