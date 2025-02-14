using System;
using System.Collections.Generic;
using CityRoots.Core.Models;

namespace CityRoots.Core.Seeding
{
    public static class CropSeeder
    {
        public static List<Crop> GetSeedData()
        {
            var crops = new List<Crop>
            {
                new Crop
                {
                    CropId = 1,
                    Name = "قمح", // Wheat
                    CurrentPrice = 15.00m,
                    ExpectedPriceChange = 16.00m, // Increases to 18
                    RiskDescription = "منخفض المخاطر بسبب الطلب المستقر",
                    CropTypeId = 1, // حبوب
                    RiskLevel = "منخفض",
                    ImageUrl = "https://example.com/images/wheat.jpg"
                },
                new Crop
                {
                    CropId = 2,
                    Name = "ذرة", // Corn
                    CurrentPrice = 14.00m,
                    ExpectedPriceChange = 11.00m, // Increases to 18
                    RiskDescription = "متوسط المخاطر بسبب الظروف الجوية",
                    CropTypeId = 1, // حبوب
                    RiskLevel = "متوسط",
                    ImageUrl = "https://example.com/images/corn.jpg"
                },
                new Crop
                {
                    CropId = 3,
                    Name = "تفاح", // Apple
                    CurrentPrice = 18.00m,
                    ExpectedPriceChange = 10.00m, // Increases to 18
                    RiskDescription = "مرتفع المخاطر بسبب تقلبات السوق",
                    CropTypeId = 2, // فاكهه
                    RiskLevel = "مرتفع",
                    ImageUrl = "https://example.com/images/apple.jpg"
                },
                new Crop
                {
                    CropId = 4,
                    Name = "برتقال", // Orange
                    CurrentPrice = 16.00m,
                    ExpectedPriceChange = 16.00m, //Increases to 18
                    RiskDescription = "منخفض المخاطر مع طلب مستقر",
                    CropTypeId = 2, // فاكهه
                    RiskLevel = "منخفض",
                    ImageUrl = "https://example.com/images/orange.jpg"
                },
                new Crop
                {
                    CropId = 5,
                    Name = "طماطم", // Tomato
                    CurrentPrice = 19.00m,
                    ExpectedPriceChange = 18.00m, // Increases to 18
                    RiskDescription = "متوسط المخاطر بسبب قيود التصدير",
                    CropTypeId = 3, // خضار
                    RiskLevel = "متوسط",
                    ImageUrl = "https://example.com/images/tomato.jpg"
                },
                new Crop
                {
                    CropId = 6,
                    Name = "خيار", // Cucumber
                    CurrentPrice = 11.00m,
                    ExpectedPriceChange = 11.00m, //Increases to 18
                    RiskDescription = "منخفض المخاطر مع إنتاج وفير",
                    CropTypeId = 3, // خضار
                    RiskLevel = "منخفض",
                    ImageUrl = "https://example.com/images/cucumber.jpg"
                },
                new Crop
                {
                    CropId = 7,
                    Name = "أرز", // Rice
                    CurrentPrice = 15.00m,
                    ExpectedPriceChange = 14.00m,  //Increases to 18
                    RiskDescription = "منخفض المخاطر مع طلب عالمي مرتفع",
                    CropTypeId = 1, // حبوب
                    RiskLevel = "منخفض",
                    ImageUrl = "https://example.com/images/rice.jpg"
                },
                new Crop
                {
                    CropId = 8,
                    Name = "شوفان", // Oats
                    CurrentPrice = 25.00m,
                    ExpectedPriceChange = 25.00m, //Increases to 18
                    RiskDescription = "منخفض المخاطر ومناسب للعديد من التطبيقات",
                    CropTypeId = 1, // حبوب
                    RiskLevel = "منخفض",
                    ImageUrl = "https://example.com/images/oats.jpg"
                },
                new Crop
                {
                    CropId = 9,
                    Name = "عنب", // Grape
                    CurrentPrice = 22.00m,
                    ExpectedPriceChange = 23.00m, //Increases to 18
                    RiskDescription = "متوسط المخاطر بسبب الأمراض والآفات",
                    CropTypeId = 2, // فاكهه
                    RiskLevel = "متوسط",
                    ImageUrl = "https://example.com/images/grape.jpg"
                },
                new Crop
                {
                    CropId = 10,
                    Name = "موز", // Banana
                    CurrentPrice = 17.00m,
                    ExpectedPriceChange = 17.00m, // Increases to 18
                    RiskDescription = "منخفض المخاطر مع طلب استهلاكي كبير",
                    CropTypeId = 2, // فاكهه
                    RiskLevel = "منخفض",
                    ImageUrl = "https://example.com/images/banana.jpg"
                },
                 new Crop
                {
                    CropId = 11,
                    Name = "بطاطس", // Potato
                    CurrentPrice = 15.00m,
                    ExpectedPriceChange = 3.00m,  //Increases to 18
                    RiskDescription = "منخفض المخاطر مع استخدامات متعددة",
                    CropTypeId = 3, // خضار
                    RiskLevel = "منخفض",
                    ImageUrl = "https://example.com/images/potato.jpg"
                },
                new Crop
                {
                    CropId = 12,
                    Name = "جزر", // Carrot
                    CurrentPrice = 15.00m,
                    ExpectedPriceChange = 3.00m, //Increases to 18
                    RiskDescription = "منخفض المخاطر ومصدر غني بالفيتامينات",
                    CropTypeId = 3, // خضار
                    RiskLevel = "منخفض",
                    ImageUrl = "https://example.com/images/carrot.jpg"
                },
                new Crop
                {
                    CropId = 13,
                    Name = "فول الصويا", // Soybean
                    CurrentPrice = 15.00m,
                    ExpectedPriceChange = 3.00m,  //Increases to 18
                    RiskDescription = "متوسط المخاطر بسبب تقلبات الأسواق العالمية",
                    CropTypeId = 1, // حبوب
                    RiskLevel = "متوسط",
                    ImageUrl = "https://example.com/images/soybean.jpg"
                },
                new Crop
                {
                    CropId = 14,
                    Name = "شعير", // Barley
                    CurrentPrice = 15.00m,
                    ExpectedPriceChange = 3.00m, //Increases to 18
                    RiskDescription = "منخفض المخاطر مع استخدامات متنوعة في الصناعة",
                    CropTypeId = 1, // حبوب
                    RiskLevel = "منخفض",
                    ImageUrl = "https://example.com/images/barley.jpg"
                },
                new Crop
                {
                    CropId = 15,
                    Name = "فراولة", // Strawberry
                    CurrentPrice = 15.00m,
                    ExpectedPriceChange = 3.00m, //Increases to 18
                    RiskDescription = "مرتفع المخاطر بسبب موسمية الإنتاج",
                    CropTypeId = 2, // فاكهه
                    RiskLevel = "مرتفع",
                    ImageUrl = "https://example.com/images/strawberry.jpg"
                },
                new Crop
                {
                    CropId = 16,
                    Name = "كمثرى", // Pear
                    CurrentPrice = 15.00m,
                    ExpectedPriceChange = 3.00m,  //Increases to 18
                    RiskDescription = "متوسط المخاطر بسبب مشاكل التخزين",
                    CropTypeId = 2, // فاكهه
                    RiskLevel = "متوسط",
                    ImageUrl = "https://example.com/images/pear.jpg"
                },
                new Crop
                {
                    CropId = 17,
                    Name = "بصل", // Onion
                    CurrentPrice = 15.00m,
                    ExpectedPriceChange = 3.00m, //Increases to 18
                    RiskDescription = "منخفض المخاطر مع طلب أساسي في الطهي",
                    CropTypeId = 3, // خضار
                    RiskLevel = "منخفض",
                    ImageUrl = "https://example.com/images/onion.jpg"
                },
                new Crop
                {
                    CropId = 18,
                    Name = "ملفوف", // Cabbage
                    CurrentPrice = 15.00m,
                    ExpectedPriceChange = 3.00m, //Increases to 18
                    RiskDescription = "منخفض المخاطر ومصدر جيد للفيتامينات",
                    CropTypeId = 3, // خضار
                    RiskLevel = "منخفض",
                    ImageUrl = "https://example.com/images/cabbage.jpg"
                },
                new Crop
                {
                    CropId = 19,
                    Name = "الذرة الرفيعة", // Sorghum
                    CurrentPrice = 15.00m,
                    ExpectedPriceChange = 3.00m, // Increases to 18
                    RiskDescription = "منخفض المخاطر، مقاومة للجفاف",
                    CropTypeId = 1, // حبوب
                    RiskLevel = "منخفض",
                    ImageUrl = "https://example.com/images/sorghum.jpg"
                },
                new Crop
                {
                    CropId = 20,
                    Name = "الجاودار", // Rye
                    CurrentPrice = 15.00m,
                    ExpectedPriceChange = 3.00m, //Increases to 18
                    RiskDescription = "منخفض المخاطر، يستخدم في الخبز وصناعة الأعلاف",
                    CropTypeId = 1, // حبوب
                    RiskLevel = "منخفض",
                    ImageUrl = "https://example.com/images/rye.jpg"
                },
                new Crop
                {
                    CropId = 21,
                    Name = "مانجو", // Mango
                    CurrentPrice = 15.00m,
                    ExpectedPriceChange = 3.00m, //Increases to 18
                    RiskDescription = "متوسط المخاطر، حساسية للطقس البارد",
                    CropTypeId = 2, // فاكهه
                    RiskLevel = "متوسط",
                    ImageUrl = "https://example.com/images/mango.jpg"
                },
                new Crop
                {
                    CropId = 22,
                    Name = "أناناس", // Pineapple
                    CurrentPrice = 15.00m,
                    ExpectedPriceChange = 3.00m, //Increases to 18
                    RiskDescription = "متوسط المخاطر، فترة نمو طويلة",
                    CropTypeId = 2, // فاكهه
                    RiskLevel = "متوسط",
                    ImageUrl = "https://example.com/images/pineapple.jpg"
                },
                new Crop
                {
                    CropId = 23,
                    Name = "فلفل", // Pepper
                    CurrentPrice = 15.00m,
                    ExpectedPriceChange = 3.00m, //Increases to 18
                    RiskDescription = "متوسط المخاطر، عرضة للأمراض",
                    CropTypeId = 3, // خضار
                    RiskLevel = "متوسط",
                    ImageUrl = "https://example.com/images/pepper.jpg"
                },
                new Crop
                {
                    CropId = 24,
                    Name = "باذنجان", // Eggplant
                    CurrentPrice = 15.00m,
                    ExpectedPriceChange = 3.00m, //Increases to 18
                    RiskDescription = "منخفض المخاطر، ينمو في الطقس الدافئ",
                    CropTypeId = 3, // خضار
                    RiskLevel = "منخفض",
                    ImageUrl = "https://example.com/images/eggplant.jpg"
                },
                new Crop
                {
                    CropId = 25,
                    Name = "العدس", // Lentils
                    CurrentPrice = 15.00m,
                    ExpectedPriceChange = 3.00m, //Increases to 18
                    RiskDescription = "منخفض المخاطر، محصول غذائي أساسي",
                    CropTypeId = 1, // حبوب
                    RiskLevel = "منخفض",
                    ImageUrl = "https://example.com/images/lentils.jpg"
                },
                new Crop
                {
                    CropId = 26,
                    Name = "الفاصوليا", // Beans
                    CurrentPrice = 15.00m,
                    ExpectedPriceChange = 3.00m, //Increases to 18
                    RiskDescription = "منخفض المخاطر، محصول غذائي أساسي",
                    CropTypeId = 1, // حبوب
                    RiskLevel = "منخفض",
                    ImageUrl = "https://example.com/images/beans.jpg"
                },
                new Crop
                {
                    CropId = 27,
                    Name = "كيوي", // Kiwi
                    CurrentPrice = 15.00m,
                    ExpectedPriceChange = 3.00m, //Increases to 18
                    RiskDescription = "مرتفع المخاطر، يتطلب مناخًا خاصًا",
                    CropTypeId = 2, // فاكهه
                    RiskLevel = "مرتفع",
                    ImageUrl = "https://example.com/images/kiwi.jpg"
                },
                new Crop
                {
                    CropId = 28,
                    Name = "خوخ", // Peach
                    CurrentPrice = 15.00m,
                    ExpectedPriceChange = 3.00m, //Increases to 18
                    RiskDescription = "متوسط المخاطر، عرضة للأمراض والآفات",
                    CropTypeId = 2, // فاكهه
                    RiskLevel = "متوسط",
                    ImageUrl = "https://example.com/images/peach.jpg"
                },
                new Crop
                {
                    CropId = 29,
                    Name = "سبانخ", // Spinach
                    CurrentPrice = 15.00m,
                    ExpectedPriceChange = 3.00m, //Increases to 18
                    RiskDescription = "منخفض المخاطر، دورة نمو قصيرة",
                    CropTypeId = 3, // خضار
                    RiskLevel = "منخفض",
                    ImageUrl = "https://example.com/images/spinach.jpg"
                },
                new Crop
                {
                    CropId = 30,
                    Name = "قرنبيط", // Cauliflower
                    CurrentPrice = 15.00m,
                    ExpectedPriceChange = 3.00m, //Increases to 18
                    RiskDescription = "متوسط المخاطر، يتطلب رعاية خاصة",
                    CropTypeId = 3, // خضار
                    RiskLevel = "متوسط",
                    ImageUrl = "https://example.com/images/cauliflower.jpg"
                },
                new Crop
                {
                    CropId = 31,
                    Name = "بنجر",
                    CurrentPrice = 15.00m,
                    ExpectedPriceChange = 3.00m, //Increases to 18
                    RiskDescription = "منخفض المخاطر، محصول جذري متعدد الاستخدامات",
                    CropTypeId = 3,
                    RiskLevel = "منخفض",
                    ImageUrl = "https://example.com/images/beetroot.jpg"
                },
                new Crop
                {
                    CropId = 32,
                    Name = "لوز",
                    CurrentPrice = 15.00m,
                    ExpectedPriceChange = 3.00m, //Increases to 18
                    RiskDescription = "مرتفع المخاطر، يتأثر بالظروف الجوية والصقيع",
                    CropTypeId = 2,
                    RiskLevel = "مرتفع",
                    ImageUrl = "https://example.com/images/almond.jpg"
                },
                new Crop
                {
                    CropId = 33,
                    Name = "جوز",
                    CurrentPrice = 15.00m,
                    ExpectedPriceChange = 3.00m, //Increases to 18
                    RiskDescription = "مرتفع المخاطر، عرضة للأمراض والآفات",
                    CropTypeId = 2,
                    RiskLevel = "مرتفع",
                    ImageUrl = "https://example.com/images/walnut.jpg"
                },
                new Crop
                {
                    CropId = 34,
                    Name = "كرنب",
                    CurrentPrice = 15.00m,
                    ExpectedPriceChange = 3.00m, //Increases to 18
                    RiskDescription = "منخفض المخاطر، محصول شتوي مقاوم للبرد",
                    CropTypeId = 3,
                    RiskLevel = "منخفض",
                    ImageUrl = "https://example.com/images/kale.jpg"
                },
                new Crop
                {
                    CropId = 35,
                    Name = "فول سوداني",
                    CurrentPrice = 15.00m,
                    ExpectedPriceChange = 3.00m, //Increases to 18
                    RiskDescription = "متوسط المخاطر، يتطلب ظروف تربة ومناخ محددة",
                    CropTypeId = 1,
                    RiskLevel = "متوسط",
                    ImageUrl = "https://example.com/images/peanut.jpg"
                },
                new Crop
                {
                    CropId = 36,
                    Name = "بندق",
                    CurrentPrice = 15.00m,
                    ExpectedPriceChange = 3.00m, //Increases to 18
                    RiskDescription = "مرتفع المخاطر، يتطلب تقليمًا خاصًا",
                    CropTypeId = 2,
                    RiskLevel = "مرتفع",
                    ImageUrl = "https://example.com/images/hazelnut.jpg"
                },
                new Crop
                {
                    CropId = 37,
                    Name = "يقطين",
                    CurrentPrice = 15.00m,
                    ExpectedPriceChange = 3.00m, //Increases to 18
                    RiskDescription = "منخفض المخاطر، ينمو بسهولة في التربة الغنية",
                    CropTypeId = 3,
                    RiskLevel = "منخفض",
                    ImageUrl = "https://example.com/images/pumpkin.jpg"
                },
                new Crop
                {
                    CropId = 38,
                    Name = "الكتان",
                    CurrentPrice = 15.00m,
                    ExpectedPriceChange = 3.00m, //Increases to 18
                    RiskDescription = "منخفض المخاطر، محصول متعدد الاستخدامات",
                    CropTypeId = 1,
                    RiskLevel = "منخفض",
                    ImageUrl = "https://example.com/images/flax.jpg"
                },
                 new Crop
                {
                    CropId = 39,
                    Name = "تين",
                    CurrentPrice = 15.00m,
                    ExpectedPriceChange = 3.00m, //Increases to 18
                    RiskDescription = "مرتفع المخاطر، يتطلب مناخًا دافئًا ومستقرًا",
                    CropTypeId = 2,
                    RiskLevel = "مرتفع",
                    ImageUrl = "https://example.com/images/fig.jpg"
                },
                new Crop
                {
                    CropId = 40,
                    Name = "رمان",
                    CurrentPrice = 15.00m,
                    ExpectedPriceChange = 3.00m, //Increases to 18
                    RiskDescription = "متوسط المخاطر، حساس للتغيرات في الرطوبة",
                    CropTypeId = 2,
                    RiskLevel = "متوسط",
                    ImageUrl = "https://example.com/images/pomegranate.jpg"
                },
                new Crop
                {
                    CropId = 41,
                    Name = "فجل",
                    CurrentPrice = 15.00m,
                    ExpectedPriceChange = 3.00m, //Increases to 18
                    RiskDescription = "منخفض المخاطر، محصول سريع النمو",
                    CropTypeId = 3,
                    RiskLevel = "منخفض",
                    ImageUrl = "https://example.com/images/radish.jpg"
                },
                 new Crop
                {
                    CropId = 42,
                    Name = "كرز",
                    CurrentPrice = 15.00m,
                    ExpectedPriceChange = 3.00m, //Increases to 18
                    RiskDescription = "مرتفع المخاطر، حساس للظروف الجوية والصقيع",
                    CropTypeId = 2,
                    RiskLevel = "مرتفع",
                    ImageUrl = "https://example.com/images/cherry.jpg"
                },
                new Crop
                {
                    CropId = 43,
                    Name = "لفت",
                    CurrentPrice = 15.00m,
                    ExpectedPriceChange = 3.00m, //Increases to 18
                    RiskDescription = "منخفض المخاطر، محصول جذري متين",
                    CropTypeId = 3,
                    RiskLevel = "منخفض",
                    ImageUrl = "https://example.com/images/turnip.jpg"
                },
                new Crop
                {
                    CropId = 44,
                    Name = "حمص",
                    CurrentPrice = 15.00m,
                    ExpectedPriceChange = 3.00m, //Increases to 18
                    RiskDescription = "منخفض المخاطر، محصول غذائي غني بالبروتين",
                    CropTypeId = 1,
                    RiskLevel = "منخفض",
                    ImageUrl = "https://example.com/images/chickpeas.jpg"
                },
                 new Crop
                {
                    CropId = 45,
                    Name = "بامية",
                    CurrentPrice = 15.00m,
                    ExpectedPriceChange = 3.00m, //Increases to 18
                    RiskDescription = "متوسط المخاطر، حساس للبرد",
                    CropTypeId = 3,
                    RiskLevel = "متوسط",
                    ImageUrl = "https://example.com/images/okra.jpg"
                },
                new Crop
                {
                    CropId = 46,
                    Name = "زيتون",
                    CurrentPrice = 15.00m,
                    ExpectedPriceChange = 3.00m, //Increases to 18
                    RiskDescription = "مرتفع المخاطر، يتطلب سنوات عديدة لإنتاج محصول",
                    CropTypeId = 2,
                    RiskLevel = "مرتفع",
                    ImageUrl = "https://example.com/images/olives.jpg"
                },
                new Crop
                {
                    CropId = 47,
                    Name = "يانسون",
                    CurrentPrice = 15.00m,
                    ExpectedPriceChange = 3.00m, //Increases to 18
                    RiskDescription = "منخفض المخاطر، يستخدم في العديد من الصناعات",
                    CropTypeId = 1,
                    RiskLevel = "منخفض",
                    ImageUrl = "https://example.com/images/anise.jpg"
                },
                new Crop
                {
                    CropId = 48,
                    Name = "زنجبيل",
                    CurrentPrice = 15.00m,
                    ExpectedPriceChange = 3.00m, //Increases to 18
                    RiskDescription = "متوسط المخاطر، يتطلب تربة دافئة ورطبة",
                    CropTypeId = 3,
                    RiskLevel = "متوسط",
                    ImageUrl = "https://example.com/images/ginger.jpg"
                },
                new Crop
                {
                    CropId = 49,
                    Name = "دخن",
                    CurrentPrice = 15.00m,
                    ExpectedPriceChange = 3.00m, //Increases to 18
                    RiskDescription = "منخفض المخاطر، محصول مقاوم للجفاف",
                    CropTypeId = 1,
                    RiskLevel = "منخفض",
                    ImageUrl = "https://example.com/images/millet.jpg"
                },
                 new Crop
                {
                    CropId = 50,
                    Name = "عنب الثعلب",
                    CurrentPrice = 15.00m,
                    ExpectedPriceChange = 3.00m, //Increases to 18
                    RiskDescription = "مرتفع المخاطر، عرضة للأمراض والآفات",
                    CropTypeId = 2,
                    RiskLevel = "مرتفع",
                    ImageUrl = "https://example.com/images/gooseberry.jpg"
                }
            };
            return crops;
        }
    }
}