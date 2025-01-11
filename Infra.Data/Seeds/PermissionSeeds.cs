using Domain.Entities.Permission;
using Infra.Data.Statics;

namespace Infra.Data.Seeds;
public class PermissionSeeds
{
    public static List<Permission> ApplicationPermissions { get; } =
    [
        #region User

        new()
        {
            ParentId = null,
            Id = 1,
            UniqueName = PermissionName.UserManagement,
            DisplayName = "مدیریت کاربران",
        },
        new()
        {
            ParentId = 1,
            Id = 2,
            UniqueName = PermissionName.UserList,
            DisplayName = "لیست کاربران"
        },
        new()
        {
            ParentId = 1,
            Id = 3,
            UniqueName = PermissionName.CreateUser,
            DisplayName = "ایجاد کاربر"
        },
        new()
        {
            ParentId = 1,
            Id = 4,
            UniqueName = PermissionName.DeleteUser,
            DisplayName = "حذف کاربر"
        },
        new()
        {
            ParentId = 1,
            Id = 5,
            UniqueName = PermissionName.UpdateUser,
            DisplayName = "به‌روزرسانی کاربر"
        },
        new()
        {
            ParentId = 1,
            Id = 6,
            UniqueName = PermissionName.UserProfile,
            DisplayName = "پروفایل کاربر"
        },

        #endregion

        #region Banners

        #region Fixed Banners

        new()
        {
            ParentId = null,
            Id = 7,
            UniqueName = PermissionName.FixedBannerManagement,
            DisplayName = "مدیریت بنرهای ثابت"
        },
        new()
        {
            ParentId = 7,
            Id = 8,
            UniqueName = PermissionName.FixedBannerList,
            DisplayName = "لیست بنرهای ثابت"
        },
        new()
        {
            ParentId = 7,
            Id = 9,
            UniqueName = PermissionName.CreateFixedBanner,
            DisplayName = "ایجاد بنر ثابت"
        },
        new()
        {
            ParentId = 7,
            Id = 10,
            UniqueName = PermissionName.DeleteFixedBanner,
            DisplayName = "حذف بنر ثابت"
        },
        new()
        {
            ParentId = 7,
            Id = 11,
            UniqueName = PermissionName.UpdateFixedBanner,
            DisplayName = "به‌روزرسانی بنر ثابت"
        },

        #endregion

        #region Slider Images

        new()
        {
            ParentId = null,
            Id = 12,
            UniqueName = PermissionName.SliderImageManagement,
            DisplayName = "مدیریت تصاویر اسلایدر"
        },
        new()
        {
            ParentId = 12,
            Id = 13,
            UniqueName = PermissionName.SliderImageList,
            DisplayName = "لیست تصاویر اسلایدر"
        },
        new()
        {
            ParentId = 12,
            Id = 14,
            UniqueName = PermissionName.CreateSliderImage,
            DisplayName = "ایجاد تصویر اسلایدر"
        },
        new()
        {
            ParentId = 12,
            Id = 15,
            UniqueName = PermissionName.DeleteSliderImage,
            DisplayName = "حذف تصویر اسلایدر"
        },
        new()
        {
            ParentId = 12,
            Id = 16,
            UniqueName = PermissionName.UpdateSliderImage,
            DisplayName = "به‌روزرسانی تصویر اسلایدر"
        },

        #endregion

        #endregion

        #region Discount

        new()
        {
            ParentId = null,
            Id = 17,
            UniqueName = PermissionName.DiscountManagement,
            DisplayName = "مدیریت تخفیف‌ها"
        },
        new()
        {
            ParentId = 17,
            Id = 18,
            UniqueName = PermissionName.DiscountList,
            DisplayName = "لیست تخفیف‌ها"
        },
        new()
        {
            ParentId = 17,
            Id = 19,
            UniqueName = PermissionName.CreateDiscount,
            DisplayName = "ایجاد تخفیف"
        },
        new()
        {
            ParentId = 17,
            Id = 20,
            UniqueName = PermissionName.DeleteDiscount,
            DisplayName = "حذف تخفیف"
        },
        new()
        {
            ParentId = 17,
            Id = 21,
            UniqueName = PermissionName.UpdateDiscount,
            DisplayName = "به‌روزرسانی تخفیف"
        },
        new()
        {
            ParentId = 17,
            Id = 22,
            UniqueName = PermissionName.AssignToUserDiscount,
            DisplayName = "اختصاص تخفیف به کاربر"
        },
        new()
        {
            ParentId = 17,
            Id = 23,
            UniqueName = PermissionName.AssignToProductDiscount,
            DisplayName = "اختصاص تخفیف به محصول"
        },

        #endregion

        #region Ticket

        new()
        {
            ParentId = null,
            Id = 24,
            UniqueName = PermissionName.TicketManagement,
            DisplayName = "مدیریت تیکت‌ها"
        },
        new()
        {
            ParentId = 24,
            Id = 25,
            UniqueName = PermissionName.TicketList,
            DisplayName = "لیست تیکت‌ها"
        },
        new()
        {
            ParentId = 24,
            Id = 26,
            UniqueName = PermissionName.DeleteTicket,
            DisplayName = "حذف تیکت"
        },
        new()
        {
            ParentId = 24,
            Id = 27,
            UniqueName = PermissionName.AnswerTicket,
            DisplayName = "پاسخ به تیکت"
        },

        #endregion

        #region Contact Us

        new()
        {
            ParentId = null,
            Id = 28,
            UniqueName = PermissionName.ContactUsManagement,
            DisplayName = "مدیریت تماس با ما"
        },
        new()
        {
            ParentId = 28,
            Id = 29,
            UniqueName = PermissionName.ContactUsList,
            DisplayName = "لیست تماس با ما"
        },
        new()
        {
            ParentId = 28,
            Id = 30,
            UniqueName = PermissionName.DeleteContactUs,
            DisplayName = "حذف تماس با ما"
        },
        new()
        {
            ParentId = 28,
            Id = 31,
            UniqueName = PermissionName.AnswerContactUs,
            DisplayName = "پاسخ به تماس با ما"
        },

        #endregion

        #region Faq

        new()
        {
            ParentId = null,
            Id = 32,
            UniqueName = PermissionName.FaqManagement,
            DisplayName = "مدیریت سوالات متداول"
        },
        new()
        {
            ParentId = 32,
            Id = 33,
            UniqueName = PermissionName.FaqList,
            DisplayName = "لیست سوالات متداول"
        },
        new()
        {
            ParentId = 32,
            Id = 34,
            UniqueName = PermissionName.CreateFaq,
            DisplayName = "ایجاد سوال متداول"
        },
        new()
        {
            ParentId = 32,
            Id = 35,
            UniqueName = PermissionName.CreateFaqCategory,
            DisplayName = "ایجاد دسته‌بندی سوالات متداول"
        },
        new()
        {
            ParentId = 32,
            Id = 36,
            UniqueName = PermissionName.DeleteFaq,
            DisplayName = "حذف سوال متداول"
        },
        new()
        {
            ParentId = 32,
            Id = 37,
            UniqueName = PermissionName.DeleteFaqCategory,
            DisplayName = "حذف دسته‌بندی سوالات متداول"
        },
        new()
        {
            ParentId = 32,
            Id = 38,
            UniqueName = PermissionName.UpdateFaq,
            DisplayName = "به‌روزرسانی سوال متداول"
        },
        new()
        {
            ParentId = 32,
            Id = 39,
            UniqueName = PermissionName.UpdateFaqCategory,
            DisplayName = "به‌روزرسانی دسته‌بندی سوالات متداول"
        },

        #endregion

        #region Notification

        new()
        {
            ParentId = null,
            Id = 40,
            UniqueName = PermissionName.NotificationManagement,
            DisplayName = "مدیریت اطلاعیه‌ها"
        },
        new()
        {
            ParentId = 40,
            Id = 41,
            UniqueName = PermissionName.CreateNotification,
            DisplayName = "ایجاد اطلاعیه"
        },

        #endregion

        #region Comment

        new()
        {
            ParentId = null,
            Id = 42,
            UniqueName = PermissionName.CommentManagement,
            DisplayName = "مدیریت نظرات"
        },
        new()
        {
            ParentId = 42,
            Id = 43,
            UniqueName = PermissionName.CommentList,
            DisplayName = "لیست نظرات"
        },
        new()
        {
            ParentId = 42,
            Id = 44,
            UniqueName = PermissionName.DeleteComment,
            DisplayName = "حذف نظر"
        },

        #endregion

        #region Product

        new()
        {
            ParentId = null,
            Id = 45,
            UniqueName = PermissionName.ProductManagement,
            DisplayName = "مدیریت محصولات"
        },
        new()
        {
            ParentId = 45,
            Id = 46,
            UniqueName = PermissionName.ProductList,
            DisplayName = "لیست محصولات"
        },
        new()
        {
            ParentId = 45,
            Id = 47,
            UniqueName = PermissionName.ProductGallery,
            DisplayName = "گالری محصولات"
        },
        new()
        {
            ParentId = 45,
            Id = 48,
            UniqueName = PermissionName.CreateProduct,
            DisplayName = "ایجاد محصول"
        },
        new()
        {
            ParentId = 45,
            Id = 49,
            UniqueName = PermissionName.DeleteProduct,
            DisplayName = "حذف محصول"
        },
        new()
        {
            ParentId = 45,
            Id = 50,
            UniqueName = PermissionName.UpdateProduct,
            DisplayName = "به‌روزرسانی محصول"
        },

        #region Specification

        new()
        {
            ParentId = 45,
            Id = 51,
            UniqueName = PermissionName.SpecificationManagement,
            DisplayName = "مدیریت مشخصات"
        },
        new()
        {
            ParentId = 51,
            Id = 52,
            UniqueName = PermissionName.SpecificationList,
            DisplayName = "لیست مشخصات"
        },
        new()
        {
            ParentId = 51,
            Id = 53,
            UniqueName = PermissionName.CreateSpecification,
            DisplayName = "ایجاد مشخصه"
        },
        new()
        {
            ParentId = 51,
            Id = 54,
            UniqueName = PermissionName.DeleteSpecification,
            DisplayName = "حذف مشخصه"
        },
        new()
        {
            ParentId = 51,
            Id = 55,
            UniqueName = PermissionName.UpdateSpecification,
            DisplayName = "به‌روزرسانی مشخصه"
        },

        #endregion

        #endregion

        #region Category

        new()
        {
            ParentId = null,
            Id = 56,
            UniqueName = PermissionName.CategoryManagement,
            DisplayName = "مدیریت دسته‌بندی‌ها"
        },
        new()
        {
            ParentId = 56,
            Id = 57,
            UniqueName = PermissionName.CategoryList,
            DisplayName = "لیست دسته‌بندی‌ها"
        },
        new()
        {
            ParentId = 56,
            Id = 58,
            UniqueName = PermissionName.CreateBaseCategory,
            DisplayName = "ایجاد دسته‌بندی اصلی"
        },
        new()
        {
            ParentId = 56,
            Id = 59,
            UniqueName = PermissionName.CreateSubCategory,
            DisplayName = "ایجاد زیردسته‌بندی"
        },
        new()
        {
            ParentId = 56,
            Id = 60,
            UniqueName = PermissionName.DeleteCategory,
            DisplayName = "حذف دسته‌بندی"
        },
        new()
        {
            ParentId = 56,
            Id = 61,
            UniqueName = PermissionName.UpdateCategory,
            DisplayName = "به‌روزرسانی دسته‌بندی"
        },

        #endregion

        #region Color

        new()
        {
            ParentId = null,
            Id = 62,
            UniqueName = PermissionName.ColorManagement,
            DisplayName = "مدیریت رنگ‌ها"
        },
        new()
        {
            ParentId = 62,
            Id = 63,
            UniqueName = PermissionName.ColorList,
            DisplayName = "لیست رنگ‌ها"
        },
        new()
        {
            ParentId = 62,
            Id = 64,
            UniqueName = PermissionName.CreateColor,
            DisplayName = "ایجاد رنگ"
        },
        new()
        {
            ParentId = 62,
            Id = 65,
            UniqueName = PermissionName.DeleteColor,
            DisplayName = "حذف رنگ"
        },

        #endregion

        #region Question

        new()
        {
            ParentId = null,
            Id = 66,
            UniqueName = PermissionName.QuestionManagement,
            DisplayName = "مدیریت سوالات"
        },
        new()
        {
            ParentId = 66,
            Id = 67,
            UniqueName = PermissionName.QuestionList,
            DisplayName = "لیست سوالات"
        },
        new()
        {
            ParentId = 66,
            Id = 68,
            UniqueName = PermissionName.AnswerQuestion,
            DisplayName = "پاسخ به سوال"
        },

        #endregion

        #region Access

        new()
        {
            ParentId = null,
            Id = 69,
            UniqueName = PermissionName.AccessManagement,
            DisplayName = "مدیریت دسترسی ها"
        },new()
        {
            ParentId = 69,
            Id = 70,
            UniqueName = PermissionName.RoleList,
            DisplayName = "لیست نقش ها"
        },new()
        {
            ParentId = 69,
            Id = 71,
            UniqueName = PermissionName.UserRoleList,
            DisplayName = "لیست کاربران"
        },new()
        {
            ParentId = 69,
            Id = 72,
            UniqueName = PermissionName.AssignRoleToUser,
            DisplayName = "اضافه کردن نقش به کاربر"
        },new()
        {
            ParentId = 69,
            Id = 73,
            UniqueName = PermissionName.CreateRole,
            DisplayName = "اضافه کردن نقش"
        },new()
        {
            ParentId = 69,
            Id = 74,
            UniqueName = PermissionName.DeleteRole,
            DisplayName = "حذف نقش"
        },new()
        {
            ParentId = 69,
            Id = 75,
            UniqueName = PermissionName.UpdateRole,
            DisplayName = "ویرایش نقش"
        },

        #endregion
    ];
    public static class RoleSeeds
    {
        public static List<Role> ApplicationRoles { get; } =
        [
            new()
            {
                Id = 3,
                RoleName = "ادمین کل",
                IsDeleted = false,
                CreateDate = DateTime.UtcNow
            }
        ];
    }
    public static class RolePermissionSeeds
    {
        public static List<RolePermissionMapping> ApplicationRolePermissionMappings { get; } =
            PermissionSeeds.ApplicationPermissions
                .Select((permission, index) => new RolePermissionMapping(3,permission.Id)
                {
                    Id = index + 1, 
                    IsDeleted = false,
                    CreateDate = DateTime.UtcNow
                })
                .ToList();
    }
}