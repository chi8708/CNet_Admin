using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CNet.Common
{
    /// <summary>
    /// 容器
    /// </summary>
    public interface ICustomeContainer
    {
        //注册
        void RegisterType<TFrom, TTo>(CustomeLifetime lifetime = CustomeLifetime.Transient) where TTo : TFrom;

        //创建
        T Resolve<T>();
    }

    public class CustomeContainer : ICustomeContainer
    {
        /// <summary>
        /// 定义参数字典
        /// </summary>
        private Dictionary<string, RegisterInfo> ContainerDicationary = new Dictionary<string, RegisterInfo>();

        /// <summary>
        /// 添加服务
        /// </summary>
        /// <typeparam name="TFrom"></typeparam>
        /// <typeparam name="TTo"></typeparam>
        /// <param name="lifetime"></param>
        public void RegisterType<TFrom, TTo>(CustomeLifetime lifetime = CustomeLifetime.Transient) where TTo : TFrom
        {
            ContainerDicationary.Add(typeof(TFrom).FullName, new RegisterInfo()
            {
                TargetType = typeof(TTo),
                LifeTime = lifetime
            });
        }

        /// <summary>
        /// 创建对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Resolve<T>()
        {
            //反射创建对象
            string abstartName = typeof(T).FullName;//得到程序集+类名
            RegisterInfo registerInfo = ContainerDicationary[abstartName];//根据类型从字典得到RegisterInfo对象
            Type type = registerInfo.TargetType;//反射
            T result = default(T);
            result = (T)this.ObjectInstance(type);
            return result;
        }
        private object ObjectInstance(Type type)
        {
            ConstructorInfo ctor = type.GetConstructors().OrderByDescending(c => c.GetParameters().Length).First();
            object oInstance = Activator.CreateInstance(type);//调用无参构造
            return oInstance;

        }

        //调用
        //public void Test() 
        //{
        //    ICustomeContainer container = new CustomeContainer();//创建容器 
        //    container.RegisterType<IService, Service>();
        //    IService service = container.Resolve<IService>();//获取对象的实例
        //    service.Login();                              
        //}
    }

    public enum CustomeLifetime
    {
        /// <summary>
        /// 瞬时
        /// </summary>
        Transient,
        /// <summary>
        /// 单例
        /// </summary>
        Singleton,
        /// <summary>
        /// 作用域
        /// </summary>
        Scope
    }

    public class RegisterInfo
    {
        /// <summary>
        /// 目标类型
        /// </summary>
        public Type TargetType { get; set; }
        /// <summary>
        /// 生命周期
        /// </summary>
        public CustomeLifetime LifeTime { get; set; }
    }



}
