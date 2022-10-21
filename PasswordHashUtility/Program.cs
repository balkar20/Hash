using PasswordHashUtility;

var crypt = new Crypt();

//if (args.Length != 2)
//{
//    Console.Write("Please pass login and password as parameters");
//    Console.ReadLine();
//    return;
//}

//var hash = crypt.EncryptPassword(args[0], args[1]);
//Console.Write(hash);


//if (args.Length != 2)
//{
//    Console.Write("Please pass login and password as parameters");
//    Console.ReadLine();
//    return;
//}

var hash = crypt.EncryptPassword("0001_TBroker", "Test!123");
Console.Write(hash);