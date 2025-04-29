using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Dronee_Chan_2.Discord_Bot.Enums
{
    enum OnboardingQuestEnums
    {
        [EnumMember(Value = "NoneOnboardingQuest")]
        NoneOnboardingQuest,

        [EnumMember(Value = "UnknownOnboardingQuest")]
        UnknownOnboardingQuest,

        [EnumMember(Value = "StartOnboardingQuest")]
        StartOnboardingQuest,

        [EnumMember(Value = "FirstQuestOnboardingQuest")]
        FirstQuestOnboardingQuest,

        [EnumMember(Value = "SecondQuestOnboardingQuest")]
        SecondQuestOnboardingQuest,

        [EnumMember(Value = "ThirdQuestOnboardingQuest")]
        ThirdQuestOnboardingQuest,

        [EnumMember(Value = "FourthQuestOnboardingQuest")]
        FourthQuestOnboardingQuest,

        [EnumMember(Value = "EndScreenOnboardingQuest")]
        EndScreenOnboardingQuest,

        [EnumMember(Value = "FirstContinueButtonOnboardingQuest")]
        FirstContinueButtonOnboardingQuest,

        [EnumMember(Value = "SecondContinueButtonOnboardingQuest")]
        SecondContinueButtonOnboardingQuest,

        [EnumMember(Value = "ThirdContinueButtonOnboardingQuest")]
        ThirdContinueButtonOnboardingQuest,

        [EnumMember(Value = "FourthContinueButtonOnboardingQuest")]
        FourthContinueButtonOnboardingQuest,

        [EnumMember(Value = "SubmitButtonOnboardingQuest")]
        SubmitButtonOnboardingQuest
    }
}
