using System;
using System.Collections.Generic;
using CityRoots.Core.Models;

namespace CityRoots.Core.Seeding
{
    public static class AiPredictSeeder
    {
        public static List<AiPredict> GetSeedData()
        {
            return new List<AiPredict>
            {
                new AiPredict { AiPredictId = -1, ArabicName = "جرب التفاح", EnglishName = "Apple Scab", Diagnosis = "بقع بنية على الثمار، بقع صفراء على الأوراق", Recommendation = "مبيدات فطرية، تقليم الأغصان المصابة", IsIll = true },
                new AiPredict { AiPredictId = -2, ArabicName = "العفن الأسود في التفاح", EnglishName = "Apple Black Rot", Diagnosis = "بقع سوداء على الثمار، تساقط الأوراق", Recommendation = "مبيدات فطرية، إزالة الأجزاء المصابة", IsIll = true },
                new AiPredict { AiPredictId = -3, ArabicName = "صدأ التفاح الناتج عن الأرز", EnglishName = "Apple Cedar Rust", Diagnosis = "بقع برتقالية على الأوراق، تشوهات في الثمار", Recommendation = "مبيدات فطرية، زراعة أصناف مقاومة", IsIll = true },
                new AiPredict { AiPredictId = -4, ArabicName = "البياض الدقيقي على الكرز", EnglishName = "Cherry powdery mildew", Diagnosis = "ظهور طبقة بيضاء دقيقية على الأوراق والبراعم والثمار.\nتوقف نمو الأوراق وتشوهها.\nتساقط الأوراق والثمار.", Recommendation = "استخدام مبيدات فطرية مناسبة.\nتقليم الأغصان المصابة.\nزراعة أصناف مقاومة.", IsIll = true },
                new AiPredict { AiPredictId = -5, ArabicName = "بقعة الورقة الرمادية على الذرة", EnglishName = "Corn Gray Leaf Spot", Diagnosis = "ظهور بقع رمادية على أوراق الذرة.\nتطور البقع إلى خطوط رمادية داكنة.\nموت الأنسجة المصابة.", Recommendation = "زراعة أصناف مقاومة.\nتدوير المحاصيل.\nاستخدام مبيدات فطرية.", IsIll = true },
                new AiPredict { AiPredictId = -6, ArabicName = "صدأ الذرة الشائع", EnglishName = "Corn Common Rust", Diagnosis = "ظهور بقع برتقالية أو بنية على سطح الأوراق.\nتطور البقع إلى بثرات تنتشر منها الأبواغ الفطرية.\nتقليل محصول الذرة.", Recommendation = "زراعة أصناف مقاومة.\nاستخدام مبيدات فطرية.\nتدوير المحاصيل.", IsIll = true },
                new AiPredict { AiPredictId = -7, ArabicName = "بقعة الورقة الشمالية على الذرة", EnglishName = "Corn Northern Leaf Blight", Diagnosis = "ظهور بقع بيضاوية الشكل على أوراق الذرة.\nتطور البقع إلى خطوط طويلة داكنة اللون.\nموت الأنسجة المصابة", Recommendation = "زراعة أصناف مقاومة.\nتدوير المحاصيل.\nاستخدام مبيدات فطرية.", IsIll = true },
                new AiPredict { AiPredictId = -8, ArabicName = "العفن الأسود على العنب", EnglishName = "Grape Black Rot", Diagnosis = "ظهور بقع بنية داكنة على الأوراق والعنقود.\nتطور البقع إلى اللون الأسود.\nموت الأنسجة المصابة.", Recommendation = "تقليم الأغصان المصابة.\nاستخدام مبيدات فطرية.\nزراعة أصناف مقاومة", IsIll = true },
                new AiPredict { AiPredictId = -9, ArabicName = "الجدري الأسود على العنب", EnglishName = "Grape Black Measles", Diagnosis = "ظهور بقع سوداء صغيرة على الأوراق.\nتطور البقع إلى قرحات غائرة.\nضعف نمو الكرمة.", Recommendation = "تقليم الأغصان المصابة.\nاستخدام مبيدات فطرية.\nزراعة أصناف مقاومة.", IsIll = true },
                new AiPredict { AiPredictId = -10, ArabicName = "مرض اصفرار الشجر الحمضي", EnglishName = "Orange Huanglongbing", Diagnosis = "اصفرار الأوراق.\nتقزم الشجرة.\nثمار صغيرة وقليلة العصير.", Recommendation = "إزالة الأشجار المصابة.\nمكافحة الحشرات الناقلة للمرض.\nزراعة أصناف مقاومة.", IsIll = true },
                new AiPredict { AiPredictId = -11, ArabicName = "البقعة البكتيرية على الدراق", EnglishName = "Peach bacterial spot", Diagnosis = "ظهور بقع صغيرة دائرية على الأوراق والثمار.\nتطور البقع إلى قرحات.\nتساقط الأوراق والثمار.", Recommendation = "تقليم الأغصان المصابة.\nاستخدام مبيدات بكتيرية.\nزراعة أصناف مقاومة.", IsIll = true },
                new AiPredict { AiPredictId = -12, ArabicName = "بقعة الورقة على العنب", EnglishName = "Grape Leaf Blight", Diagnosis = "ظهور بقع بنية على الأوراق.\nتطور البقع إلى مناطق ميتة.\nتساقط الأوراق.", Recommendation = "تقليم الأغصان المصابة.\nاستخدام مبيدات فطرية.\nزراعة أصناف مقاومة.", IsIll = true },
                new AiPredict { AiPredictId = -13, ArabicName = "بقعة بكتيرية على الفلفل", EnglishName = "Pepper Bacterial Spot", Diagnosis = "بقع صغيرة داكنة مائية على الأوراق والثمار.", Recommendation = "مبيدات نحاسية، تناوب المحاصيل، أصناف مقاومة.", IsIll = true },
                new AiPredict { AiPredictId = -14, ArabicName = "البياض المبكر على البطاطس", EnglishName = "Potato Early Blight", Diagnosis = "بقع كبيرة بنية داكنة على الأوراق مع حلقات متحدة المركز.", Recommendation = "مبيدات فطرية، تناوب المحاصيل، أصناف مقاومة.", IsIll = true },
                new AiPredict { AiPredictId = -15, ArabicName = "البياض المتأخر على البطاطس", EnglishName = "Potato Late Blight", Diagnosis = "آفات مائية على الأوراق والساق، نمو فطري أبيض على الجانب السفلي من الأوراق.", Recommendation = "مبيدات فطرية، تناوب المحاصيل، أصناف مقاومة.", IsIll = true },
                new AiPredict { AiPredictId = -16, ArabicName = "البياض الدقيقي على القرع", EnglishName = "Squash Powdery Mildew", Diagnosis = "نمو أبيض بودري على الأوراق والثمار.", Recommendation = "مبيدات فطرية، أصناف مقاومة، تهوية جيدة.", IsIll = true },
                new AiPredict { AiPredictId = -17, ArabicName = "حروق أوراق الفراولة", EnglishName = "Strawberry Leaf Scorch", Diagnosis = "حواف أوراق بنية ميتة.", Recommendation = "مبيدات فطرية، ري مناسب، تهوية جيدة.", IsIll = true },
                new AiPredict { AiPredictId = -18, ArabicName = "بقعة بكتيرية على الطماطم", EnglishName = "Tomato Bacterial Spot", Diagnosis = "بقع صغيرة داكنة مائية على الأوراق والثمار.", Recommendation = "مبيدات نحاسية، تناوب المحاصيل، أصناف مقاومة.", IsIll = true },
                new AiPredict { AiPredictId = -19, ArabicName = "البياض المبكر على الطماطم", EnglishName = "Tomato Early Blight", Diagnosis = "بقع كبيرة بنية داكنة على الأوراق مع حلقات متحدة المركز.", Recommendation = "مبيدات فطرية، تناوب المحاصيل، أصناف مقاومة.", IsIll = true },
                new AiPredict { AiPredictId = -20, ArabicName = "البياض المتأخر على الطماطم", EnglishName = "Tomato Late Blight", Diagnosis = "آفات مائية على الأوراق والساق، نمو فطري أبيض على الجانب السفلي من الأوراق.", Recommendation = "مبيدات فطرية، تناوب المحاصيل، أصناف مقاومة.", IsIll = true },
                new AiPredict { AiPredictId = -21, ArabicName = "العفن الورقي على الطماطم", EnglishName = "Tomato Leaf Mold", Diagnosis = "بقع صفراء بنية على السطح العلوي للأوراق، عفن رمادي على السطح السفلي للأوراق.", Recommendation = "مبيدات فطرية، تهوية جيدة، تجنب الري العلوي.", IsIll = true },
                new AiPredict { AiPredictId = -22, ArabicName = "بقعة سيبتوريا على الطماطم", EnglishName = "Tomato Septoria Leaf Spot", Diagnosis = "بقع صغيرة بنية داكنة مع مراكز بيضاء على الأوراق.", Recommendation = "مبيدات فطرية، تناوب المحاصيل، أصناف مقاومة.", IsIll = true },
                new AiPredict { AiPredictId = -23, ArabicName = "سوس العنكبوت ذو البقعتين", EnglishName = "Tomato Spider Mites", Diagnosis = "حشرات صغيرة صفراء خضراء تمتص العصارة من الأوراق، مما يسبب بقعًا صفراء وتلونًا.", Recommendation = "صابون حشري، حشرات مفترسة، تجنب استخدام المبيدات.", IsIll = true },
                new AiPredict { AiPredictId = -24, ArabicName = "بقعة الهدف على الطماطم", EnglishName = "Tomato Target Spot", Diagnosis = "بقع كبيرة ذات حلقات متحدة المركز على الأوراق.", Recommendation = "مبيدات فطرية، تناوب المحاصيل، أصناف مقاومة.", IsIll = true },
                new AiPredict { AiPredictId = -25, ArabicName = "فيروس موزاييك الطماطم", EnglishName = "Tomato Mosaic Virus", Diagnosis = "أوراق مرقطة وصفراء ومشوهة، نمو متقزم.", Recommendation = "بذور خالية من الأمراض، نظافة، مكافحة الحشرات.", IsIll = true },
                new AiPredict { AiPredictId = -26, ArabicName = "فيروس تجعد الأوراق الصفراء للطماطم", EnglishName = "Tomato Yellow Leaf Curl Virus", Diagnosis = "اصفرار وتجعد وانحناء أوراق لأسفل.", Recommendation = "مكافحة الحشرات (ذبابة بيضاء)، أصناف مقاومة.", IsIll = true },
                new AiPredict { AiPredictId = -27, ArabicName = "طماطم صحية", EnglishName = "Tomato healthy", Diagnosis = null, Recommendation = null, IsIll = false },
                new AiPredict { AiPredictId = -28, ArabicName = "فراولة صحية", EnglishName = "Strawberry healthy", Diagnosis = null, Recommendation = null, IsIll = false },
                new AiPredict { AiPredictId = -29, ArabicName = "فول صويا صحي", EnglishName = "Soybean healthy", Diagnosis = null, Recommendation = null, IsIll = false },
                new AiPredict { AiPredictId = -30, ArabicName = "توت أحمر صحي", EnglishName = "Raspberry healthy", Diagnosis = null, Recommendation = null, IsIll = false },
                new AiPredict { AiPredictId = -31, ArabicName = "بطاطس صحية", EnglishName = "Potato healthy", Diagnosis = null, Recommendation = null, IsIll = false },
                new AiPredict { AiPredictId = -32, ArabicName = "فلفل صحي", EnglishName = "Pepper healthy", Diagnosis = null, Recommendation = null, IsIll = false },
                new AiPredict { AiPredictId = -33, ArabicName = "خوخ صحي", EnglishName = "Peach healthy", Diagnosis = null, Recommendation = null, IsIll = false },
                new AiPredict { AiPredictId = -34, ArabicName = "عنب صحي", EnglishName = "Grape healthy", Diagnosis = null, Recommendation = null, IsIll = false },
                new AiPredict { AiPredictId = -35, ArabicName = "ذرة صحية", EnglishName = "Corn healthy", Diagnosis = null, Recommendation = null, IsIll = false },
                new AiPredict { AiPredictId = -36, ArabicName = "كرز صحي", EnglishName = "Cherry healthy", Diagnosis = null, Recommendation = null, IsIll = false },
                new AiPredict { AiPredictId = -37, ArabicName = "توت أرزق صحي", EnglishName = "Blueberry healthy", Diagnosis = null, Recommendation = null, IsIll = false },
                new AiPredict { AiPredictId = -38, ArabicName = "تفاح صحي", EnglishName = "Apple healthy", Diagnosis = null, Recommendation = null, IsIll = false },
                new AiPredict { AiPredictId = -39, ArabicName = "صورة بدون أوراق", EnglishName = "Background without leaves", Diagnosis = null, Recommendation = null, IsIll = false }
            };
        }
    }
}