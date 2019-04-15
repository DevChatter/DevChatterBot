using System;
using System.Linq.Expressions;

namespace DevChatter.Bot.Modules.WastefulGame.Model.Specifications
{
    public class ShopItemPolicy : GameDataPolicy<ShopItem>
    {
        protected ShopItemPolicy(Expression<Func<ShopItem, bool>> expression)
            : base(expression)
        {
        }

        public static ShopItemPolicy All()
        {
            return new ShopItemPolicy(x => true);
        }
        public static ShopItemPolicy ById(int id)
        {
            return new ShopItemPolicy(x => x.Id == id);
        }
    }
}
