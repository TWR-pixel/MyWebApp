using MyWebApp.Data.Entities;

namespace MyWebApp.Data;

public class DatabaseInitializer
{
    public static void Init(NorthwindContext context)
    {
        //var image1 = new Image(null, "/images/cat_1.jpg", true);
        //var image2 = new Image(null, "/images/cat_2.jpg", true);
        //var image3 = new Image(null, "/images/cat_3.jpg", true);

        //var group = new Group("Street", new List<Image> {image1, image2, image3});

        var role = new Role
        {
            Name = "Admin"
        };

        var admin = new User("Admin123", "User123", role);

        context.Roles.Add(role);
        //context.Images.AddRange(image1, image2, image3);
        context.Users.Add(admin);
        //context.Groups.Add(group);


        context.SaveChanges();
    }
}