using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CityRoots.EF.Migrations
{
    /// <inheritdoc />
    public partial class addSeedForCrops : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Crops",
                columns: new[] { "CropId", "CropTypeId", "CurrentPrice", "ExpectedPriceChange", "ImageUrl", "Name", "RiskDescription", "RiskLevel" },
                values: new object[,]
                {
                    { 5, 1, 15.00m, 16.00m, "https://example.com/images/wheat.jpg", "قمح", "منخفض المخاطر بسبب الطلب المستقر", "منخفض" },
                    { 2, 1, 14.00m, 11.00m, "https://example.com/images/corn.jpg", "ذرة", "متوسط المخاطر بسبب الظروف الجوية", "متوسط" },
                    { 3, 2, 18.00m, 10.00m, "https://example.com/images/apple.jpg", "تفاح", "مرتفع المخاطر بسبب تقلبات السوق", "مرتفع" },
                    { 4, 2, 16.00m, 16.00m, "https://example.com/images/orange.jpg", "برتقال", "منخفض المخاطر مع طلب مستقر", "منخفض" },
                    { 6, 3, 11.00m, 11.00m, "https://example.com/images/cucumber.jpg", "خيار", "منخفض المخاطر مع إنتاج وفير", "منخفض" },
                    { 7, 1, 15.00m, 14.00m, "https://example.com/images/rice.jpg", "أرز", "منخفض المخاطر مع طلب عالمي مرتفع", "منخفض" },
                    { 8, 1, 25.00m, 25.00m, "https://example.com/images/oats.jpg", "شوفان", "منخفض المخاطر ومناسب للعديد من التطبيقات", "منخفض" },
                    { 9, 2, 22.00m, 23.00m, "https://example.com/images/grape.jpg", "عنب", "متوسط المخاطر بسبب الأمراض والآفات", "متوسط" },
                    { 10, 2, 17.00m, 17.00m, "https://example.com/images/banana.jpg", "موز", "منخفض المخاطر مع طلب استهلاكي كبير", "منخفض" },
                    { 11, 3, 15.00m, 3.00m, "https://example.com/images/potato.jpg", "بطاطس", "منخفض المخاطر مع استخدامات متعددة", "منخفض" },
                    { 12, 3, 15.00m, 3.00m, "https://example.com/images/carrot.jpg", "جزر", "منخفض المخاطر ومصدر غني بالفيتامينات", "منخفض" },
                    { 13, 1, 15.00m, 3.00m, "https://example.com/images/soybean.jpg", "فول الصويا", "متوسط المخاطر بسبب تقلبات الأسواق العالمية", "متوسط" },
                    { 14, 1, 15.00m, 3.00m, "https://example.com/images/barley.jpg", "شعير", "منخفض المخاطر مع استخدامات متنوعة في الصناعة", "منخفض" },
                    { 15, 2, 15.00m, 3.00m, "https://example.com/images/strawberry.jpg", "فراولة", "مرتفع المخاطر بسبب موسمية الإنتاج", "مرتفع" },
                    { 16, 2, 15.00m, 3.00m, "https://example.com/images/pear.jpg", "كمثرى", "متوسط المخاطر بسبب مشاكل التخزين", "متوسط" },
                    { 17, 3, 15.00m, 3.00m, "https://example.com/images/onion.jpg", "بصل", "منخفض المخاطر مع طلب أساسي في الطهي", "منخفض" },
                    { 18, 3, 15.00m, 3.00m, "https://example.com/images/cabbage.jpg", "ملفوف", "منخفض المخاطر ومصدر جيد للفيتامينات", "منخفض" },
                    { 19, 1, 15.00m, 3.00m, "https://example.com/images/sorghum.jpg", "الذرة الرفيعة", "منخفض المخاطر، مقاومة للجفاف", "منخفض" },
                    { 20, 1, 15.00m, 3.00m, "https://example.com/images/rye.jpg", "الجاودار", "منخفض المخاطر، يستخدم في الخبز وصناعة الأعلاف", "منخفض" },
                    { 21, 2, 15.00m, 3.00m, "https://example.com/images/mango.jpg", "مانجو", "متوسط المخاطر، حساسية للطقس البارد", "متوسط" },
                    { 22, 2, 15.00m, 3.00m, "https://example.com/images/pineapple.jpg", "أناناس", "متوسط المخاطر، فترة نمو طويلة", "متوسط" },
                    { 23, 3, 15.00m, 3.00m, "https://example.com/images/pepper.jpg", "فلفل", "متوسط المخاطر، عرضة للأمراض", "متوسط" },
                    { 24, 3, 15.00m, 3.00m, "https://example.com/images/eggplant.jpg", "باذنجان", "منخفض المخاطر، ينمو في الطقس الدافئ", "منخفض" },
                    { 25, 1, 15.00m, 3.00m, "https://example.com/images/lentils.jpg", "العدس", "منخفض المخاطر، محصول غذائي أساسي", "منخفض" },
                    { 26, 1, 15.00m, 3.00m, "https://example.com/images/beans.jpg", "الفاصوليا", "منخفض المخاطر، محصول غذائي أساسي", "منخفض" },
                    { 27, 2, 15.00m, 3.00m, "https://example.com/images/kiwi.jpg", "كيوي", "مرتفع المخاطر، يتطلب مناخًا خاصًا", "مرتفع" },
                    { 28, 2, 15.00m, 3.00m, "https://example.com/images/peach.jpg", "خوخ", "متوسط المخاطر، عرضة للأمراض والآفات", "متوسط" },
                    { 29, 3, 15.00m, 3.00m, "https://example.com/images/spinach.jpg", "سبانخ", "منخفض المخاطر، دورة نمو قصيرة", "منخفض" },
                    { 30, 3, 15.00m, 3.00m, "https://example.com/images/cauliflower.jpg", "قرنبيط", "متوسط المخاطر، يتطلب رعاية خاصة", "متوسط" },
                    { 31, 3, 15.00m, 3.00m, "https://example.com/images/beetroot.jpg", "بنجر", "منخفض المخاطر، محصول جذري متعدد الاستخدامات", "منخفض" },
                    { 32, 2, 15.00m, 3.00m, "https://example.com/images/almond.jpg", "لوز", "مرتفع المخاطر، يتأثر بالظروف الجوية والصقيع", "مرتفع" },
                    { 33, 2, 15.00m, 3.00m, "https://example.com/images/walnut.jpg", "جوز", "مرتفع المخاطر، عرضة للأمراض والآفات", "مرتفع" },
                    { 34, 3, 15.00m, 3.00m, "https://example.com/images/kale.jpg", "كرنب", "منخفض المخاطر، محصول شتوي مقاوم للبرد", "منخفض" },
                    { 35, 1, 15.00m, 3.00m, "https://example.com/images/peanut.jpg", "فول سوداني", "متوسط المخاطر، يتطلب ظروف تربة ومناخ محددة", "متوسط" },
                    { 36, 2, 15.00m, 3.00m, "https://example.com/images/hazelnut.jpg", "بندق", "مرتفع المخاطر، يتطلب تقليمًا خاصًا", "مرتفع" },
                    { 37, 3, 15.00m, 3.00m, "https://example.com/images/pumpkin.jpg", "يقطين", "منخفض المخاطر، ينمو بسهولة في التربة الغنية", "منخفض" },
                    { 38, 1, 15.00m, 3.00m, "https://example.com/images/flax.jpg", "الكتان", "منخفض المخاطر، محصول متعدد الاستخدامات", "منخفض" },
                    { 39, 2, 15.00m, 3.00m, "https://example.com/images/fig.jpg", "تين", "مرتفع المخاطر، يتطلب مناخًا دافئًا ومستقرًا", "مرتفع" },
                    { 40, 2, 15.00m, 3.00m, "https://example.com/images/pomegranate.jpg", "رمان", "متوسط المخاطر، حساس للتغيرات في الرطوبة", "متوسط" },
                    { 41, 3, 15.00m, 3.00m, "https://example.com/images/radish.jpg", "فجل", "منخفض المخاطر، محصول سريع النمو", "منخفض" },
                    { 42, 2, 15.00m, 3.00m, "https://example.com/images/cherry.jpg", "كرز", "مرتفع المخاطر، حساس للظروف الجوية والصقيع", "مرتفع" },
                    { 43, 3, 15.00m, 3.00m, "https://example.com/images/turnip.jpg", "لفت", "منخفض المخاطر، محصول جذري متين", "منخفض" },
                    { 44, 1, 15.00m, 3.00m, "https://example.com/images/chickpeas.jpg", "حمص", "منخفض المخاطر، محصول غذائي غني بالبروتين", "منخفض" },
                    { 45, 3, 15.00m, 3.00m, "https://example.com/images/okra.jpg", "بامية", "متوسط المخاطر، حساس للبرد", "متوسط" },
                    { 46, 2, 15.00m, 3.00m, "https://example.com/images/olives.jpg", "زيتون", "مرتفع المخاطر، يتطلب سنوات عديدة لإنتاج محصول", "مرتفع" },
                    { 47, 1, 15.00m, 3.00m, "https://example.com/images/anise.jpg", "يانسون", "منخفض المخاطر، يستخدم في العديد من الصناعات", "منخفض" },
                    { 48, 3, 15.00m, 3.00m, "https://example.com/images/ginger.jpg", "زنجبيل", "متوسط المخاطر، يتطلب تربة دافئة ورطبة", "متوسط" },
                    { 49, 1, 15.00m, 3.00m, "https://example.com/images/millet.jpg", "دخن", "منخفض المخاطر، محصول مقاوم للجفاف", "منخفض" },
                    { 50, 2, 15.00m, 3.00m, "https://example.com/images/gooseberry.jpg", "عنب الثعلب", "مرتفع المخاطر، عرضة للأمراض والآفات", "مرتفع" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 50);
        }
    }
}
