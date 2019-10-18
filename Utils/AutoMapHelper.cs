using System.Linq;

namespace M.Core.Utils
{
    public static class AutoMapHelper
    {
        /// <summary>
        /// 将一个实体类复制到另一个实体类
        /// </summary>
        /// <param name="objectsrc">源实体类</param>
        /// <param name="objectdest">复制到的实体类</param>
        /// <param name="excudeFields">不复制的属性</param>
        public static void EntityToEntity(object objectsrc, object objectdest, params string[] excudeFields)
        {
            var sourceType = objectsrc.GetType();
            var destType = objectdest.GetType();
            foreach (var item in destType.GetProperties())
            {
                if (excudeFields.Any(x => x.ToUpper() == item.Name.ToUpper()))
                {
                    continue;
                }

                item.SetValue(objectdest, sourceType.GetProperty(item.ToString().ToLower()).GetValue(objectsrc, null), null);
            }
        }
    }
}
