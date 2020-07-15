using System.Collections.Generic;

namespace Mitheti.Wpf.Models
{
    // лучше убрать эту модель и добавлять в каждую Localization руками
    // иерархия ради иерархии получается
    // такие наследования лучше использовать
    // в случаях когда тебе нужно структурно или поведенчески составлять композицию из моделей
    // а здесь оно используется ради одного поля
    public class BaseModel
    {
        public Dictionary<string, string> Localization { get; set; }
    }
}
