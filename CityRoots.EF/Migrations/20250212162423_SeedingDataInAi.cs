using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CityRoots.EF.Migrations
{
    /// <inheritdoc />
    public partial class SeedingDataInAi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AiPredicts",
                columns: new[] { "AiPredictId", "ArabicName", "Diagnosis", "EnglishName", "IsIll", "Recommendation" },
                values: new object[,]
                {
                    { -39, "صورة بدون أوراق", null, "Background without leaves", false, null },
                    { -38, "تفاح صحي", null, "Apple healthy", false, null },
                    { -37, "توت أرزق صحي", null, "Blueberry healthy", false, null },
                    { -36, "كرز صحي", null, "Cherry healthy", false, null },
                    { -35, "ذرة صحية", null, "Corn healthy", false, null },
                    { -34, "عنب صحي", null, "Grape healthy", false, null },
                    { -33, "خوخ صحي", null, "Peach healthy", false, null },
                    { -32, "فلفل صحي", null, "Pepper healthy", false, null },
                    { -31, "بطاطس صحية", null, "Potato healthy", false, null },
                    { -30, "توت أحمر صحي", null, "Raspberry healthy", false, null },
                    { -29, "فول صويا صحي", null, "Soybean healthy", false, null },
                    { -28, "فراولة صحية", null, "Strawberry healthy", false, null },
                    { -27, "طماطم صحية", null, "Tomato healthy", false, null },
                    { -26, "فيروس تجعد الأوراق الصفراء للطماطم", "اصفرار وتجعد وانحناء أوراق لأسفل.", "Tomato Yellow Leaf Curl Virus", true, "مكافحة الحشرات (ذبابة بيضاء)، أصناف مقاومة." },
                    { -25, "فيروس موزاييك الطماطم", "أوراق مرقطة وصفراء ومشوهة، نمو متقزم.", "Tomato Mosaic Virus", true, "بذور خالية من الأمراض، نظافة، مكافحة الحشرات." },
                    { -24, "بقعة الهدف على الطماطم", "بقع كبيرة ذات حلقات متحدة المركز على الأوراق.", "Tomato Target Spot", true, "مبيدات فطرية، تناوب المحاصيل، أصناف مقاومة." },
                    { -23, "سوس العنكبوت ذو البقعتين", "حشرات صغيرة صفراء خضراء تمتص العصارة من الأوراق، مما يسبب بقعًا صفراء وتلونًا.", "Tomato Spider Mites", true, "صابون حشري، حشرات مفترسة، تجنب استخدام المبيدات." },
                    { -22, "بقعة سيبتوريا على الطماطم", "بقع صغيرة بنية داكنة مع مراكز بيضاء على الأوراق.", "Tomato Septoria Leaf Spot", true, "مبيدات فطرية، تناوب المحاصيل، أصناف مقاومة." },
                    { -21, "العفن الورقي على الطماطم", "بقع صفراء بنية على السطح العلوي للأوراق، عفن رمادي على السطح السفلي للأوراق.", "Tomato Leaf Mold", true, "مبيدات فطرية، تهوية جيدة، تجنب الري العلوي." },
                    { -20, "البياض المتأخر على الطماطم", "آفات مائية على الأوراق والساق، نمو فطري أبيض على الجانب السفلي من الأوراق.", "Tomato Late Blight", true, "مبيدات فطرية، تناوب المحاصيل، أصناف مقاومة." },
                    { -19, "البياض المبكر على الطماطم", "بقع كبيرة بنية داكنة على الأوراق مع حلقات متحدة المركز.", "Tomato Early Blight", true, "مبيدات فطرية، تناوب المحاصيل، أصناف مقاومة." },
                    { -18, "بقعة بكتيرية على الطماطم", "بقع صغيرة داكنة مائية على الأوراق والثمار.", "Tomato Bacterial Spot", true, "مبيدات نحاسية، تناوب المحاصيل، أصناف مقاومة." },
                    { -17, "حروق أوراق الفراولة", "حواف أوراق بنية ميتة.", "Strawberry Leaf Scorch", true, "مبيدات فطرية، ري مناسب، تهوية جيدة." },
                    { -16, "البياض الدقيقي على القرع", "نمو أبيض بودري على الأوراق والثمار.", "Squash Powdery Mildew", true, "مبيدات فطرية، أصناف مقاومة، تهوية جيدة." },
                    { -15, "البياض المتأخر على البطاطس", "آفات مائية على الأوراق والساق، نمو فطري أبيض على الجانب السفلي من الأوراق.", "Potato Late Blight", true, "مبيدات فطرية، تناوب المحاصيل، أصناف مقاومة." },
                    { -14, "البياض المبكر على البطاطس", "بقع كبيرة بنية داكنة على الأوراق مع حلقات متحدة المركز.", "Potato Early Blight", true, "مبيدات فطرية، تناوب المحاصيل، أصناف مقاومة." },
                    { -13, "بقعة بكتيرية على الفلفل", "بقع صغيرة داكنة مائية على الأوراق والثمار.", "Pepper Bacterial Spot", true, "مبيدات نحاسية، تناوب المحاصيل، أصناف مقاومة." },
                    { -12, "بقعة الورقة على العنب", "ظهور بقع بنية على الأوراق.\nتطور البقع إلى مناطق ميتة.\nتساقط الأوراق.", "Grape Leaf Blight", true, "تقليم الأغصان المصابة.\nاستخدام مبيدات فطرية.\nزراعة أصناف مقاومة." },
                    { -11, "البقعة البكتيرية على الدراق", "ظهور بقع صغيرة دائرية على الأوراق والثمار.\nتطور البقع إلى قرحات.\nتساقط الأوراق والثمار.", "Peach bacterial spot", true, "تقليم الأغصان المصابة.\nاستخدام مبيدات بكتيرية.\nزراعة أصناف مقاومة." },
                    { -10, "مرض اصفرار الشجر الحمضي", "اصفرار الأوراق.\nتقزم الشجرة.\nثمار صغيرة وقليلة العصير.", "Orange Huanglongbing", true, "إزالة الأشجار المصابة.\nمكافحة الحشرات الناقلة للمرض.\nزراعة أصناف مقاومة." },
                    { -9, "الجدري الأسود على العنب", "ظهور بقع سوداء صغيرة على الأوراق.\nتطور البقع إلى قرحات غائرة.\nضعف نمو الكرمة.", "Grape Black Measles", true, "تقليم الأغصان المصابة.\nاستخدام مبيدات فطرية.\nزراعة أصناف مقاومة." },
                    { -8, "العفن الأسود على العنب", "ظهور بقع بنية داكنة على الأوراق والعنقود.\nتطور البقع إلى اللون الأسود.\nموت الأنسجة المصابة.", "Grape Black Rot", true, "تقليم الأغصان المصابة.\nاستخدام مبيدات فطرية.\nزراعة أصناف مقاومة" },
                    { -7, "بقعة الورقة الشمالية على الذرة", "ظهور بقع بيضاوية الشكل على أوراق الذرة.\nتطور البقع إلى خطوط طويلة داكنة اللون.\nموت الأنسجة المصابة", "Corn Northern Leaf Blight", true, "زراعة أصناف مقاومة.\nتدوير المحاصيل.\nاستخدام مبيدات فطرية." },
                    { -6, "صدأ الذرة الشائع", "ظهور بقع برتقالية أو بنية على سطح الأوراق.\nتطور البقع إلى بثرات تنتشر منها الأبواغ الفطرية.\nتقليل محصول الذرة.", "Corn Common Rust", true, "زراعة أصناف مقاومة.\nاستخدام مبيدات فطرية.\nتدوير المحاصيل." },
                    { -5, "بقعة الورقة الرمادية على الذرة", "ظهور بقع رمادية على أوراق الذرة.\nتطور البقع إلى خطوط رمادية داكنة.\nموت الأنسجة المصابة.", "Corn Gray Leaf Spot", true, "زراعة أصناف مقاومة.\nتدوير المحاصيل.\nاستخدام مبيدات فطرية." },
                    { -4, "البياض الدقيقي على الكرز", "ظهور طبقة بيضاء دقيقية على الأوراق والبراعم والثمار.\nتوقف نمو الأوراق وتشوهها.\nتساقط الأوراق والثمار.", "Cherry powdery mildew", true, "استخدام مبيدات فطرية مناسبة.\nتقليم الأغصان المصابة.\nزراعة أصناف مقاومة." },
                    { -3, "صدأ التفاح الناتج عن الأرز", "بقع برتقالية على الأوراق، تشوهات في الثمار", "Apple Cedar Rust", true, "مبيدات فطرية، زراعة أصناف مقاومة" },
                    { -2, "العفن الأسود في التفاح", "بقع سوداء على الثمار، تساقط الأوراق", "Apple Black Rot", true, "مبيدات فطرية، إزالة الأجزاء المصابة" },
                    { -1, "جرب التفاح", "بقع بنية على الثمار، بقع صفراء على الأوراق", "Apple Scab", true, "مبيدات فطرية، تقليم الأغصان المصابة" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AiPredicts",
                keyColumn: "AiPredictId",
                keyValue: -39);

            migrationBuilder.DeleteData(
                table: "AiPredicts",
                keyColumn: "AiPredictId",
                keyValue: -38);

            migrationBuilder.DeleteData(
                table: "AiPredicts",
                keyColumn: "AiPredictId",
                keyValue: -37);

            migrationBuilder.DeleteData(
                table: "AiPredicts",
                keyColumn: "AiPredictId",
                keyValue: -36);

            migrationBuilder.DeleteData(
                table: "AiPredicts",
                keyColumn: "AiPredictId",
                keyValue: -35);

            migrationBuilder.DeleteData(
                table: "AiPredicts",
                keyColumn: "AiPredictId",
                keyValue: -34);

            migrationBuilder.DeleteData(
                table: "AiPredicts",
                keyColumn: "AiPredictId",
                keyValue: -33);

            migrationBuilder.DeleteData(
                table: "AiPredicts",
                keyColumn: "AiPredictId",
                keyValue: -32);

            migrationBuilder.DeleteData(
                table: "AiPredicts",
                keyColumn: "AiPredictId",
                keyValue: -31);

            migrationBuilder.DeleteData(
                table: "AiPredicts",
                keyColumn: "AiPredictId",
                keyValue: -30);

            migrationBuilder.DeleteData(
                table: "AiPredicts",
                keyColumn: "AiPredictId",
                keyValue: -29);

            migrationBuilder.DeleteData(
                table: "AiPredicts",
                keyColumn: "AiPredictId",
                keyValue: -28);

            migrationBuilder.DeleteData(
                table: "AiPredicts",
                keyColumn: "AiPredictId",
                keyValue: -27);

            migrationBuilder.DeleteData(
                table: "AiPredicts",
                keyColumn: "AiPredictId",
                keyValue: -26);

            migrationBuilder.DeleteData(
                table: "AiPredicts",
                keyColumn: "AiPredictId",
                keyValue: -25);

            migrationBuilder.DeleteData(
                table: "AiPredicts",
                keyColumn: "AiPredictId",
                keyValue: -24);

            migrationBuilder.DeleteData(
                table: "AiPredicts",
                keyColumn: "AiPredictId",
                keyValue: -23);

            migrationBuilder.DeleteData(
                table: "AiPredicts",
                keyColumn: "AiPredictId",
                keyValue: -22);

            migrationBuilder.DeleteData(
                table: "AiPredicts",
                keyColumn: "AiPredictId",
                keyValue: -21);

            migrationBuilder.DeleteData(
                table: "AiPredicts",
                keyColumn: "AiPredictId",
                keyValue: -20);

            migrationBuilder.DeleteData(
                table: "AiPredicts",
                keyColumn: "AiPredictId",
                keyValue: -19);

            migrationBuilder.DeleteData(
                table: "AiPredicts",
                keyColumn: "AiPredictId",
                keyValue: -18);

            migrationBuilder.DeleteData(
                table: "AiPredicts",
                keyColumn: "AiPredictId",
                keyValue: -17);

            migrationBuilder.DeleteData(
                table: "AiPredicts",
                keyColumn: "AiPredictId",
                keyValue: -16);

            migrationBuilder.DeleteData(
                table: "AiPredicts",
                keyColumn: "AiPredictId",
                keyValue: -15);

            migrationBuilder.DeleteData(
                table: "AiPredicts",
                keyColumn: "AiPredictId",
                keyValue: -14);

            migrationBuilder.DeleteData(
                table: "AiPredicts",
                keyColumn: "AiPredictId",
                keyValue: -13);

            migrationBuilder.DeleteData(
                table: "AiPredicts",
                keyColumn: "AiPredictId",
                keyValue: -12);

            migrationBuilder.DeleteData(
                table: "AiPredicts",
                keyColumn: "AiPredictId",
                keyValue: -11);

            migrationBuilder.DeleteData(
                table: "AiPredicts",
                keyColumn: "AiPredictId",
                keyValue: -10);

            migrationBuilder.DeleteData(
                table: "AiPredicts",
                keyColumn: "AiPredictId",
                keyValue: -9);

            migrationBuilder.DeleteData(
                table: "AiPredicts",
                keyColumn: "AiPredictId",
                keyValue: -8);

            migrationBuilder.DeleteData(
                table: "AiPredicts",
                keyColumn: "AiPredictId",
                keyValue: -7);

            migrationBuilder.DeleteData(
                table: "AiPredicts",
                keyColumn: "AiPredictId",
                keyValue: -6);

            migrationBuilder.DeleteData(
                table: "AiPredicts",
                keyColumn: "AiPredictId",
                keyValue: -5);

            migrationBuilder.DeleteData(
                table: "AiPredicts",
                keyColumn: "AiPredictId",
                keyValue: -4);

            migrationBuilder.DeleteData(
                table: "AiPredicts",
                keyColumn: "AiPredictId",
                keyValue: -3);

            migrationBuilder.DeleteData(
                table: "AiPredicts",
                keyColumn: "AiPredictId",
                keyValue: -2);

            migrationBuilder.DeleteData(
                table: "AiPredicts",
                keyColumn: "AiPredictId",
                keyValue: -1);
        }
    }
}
