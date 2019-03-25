using System;
using System.Collections.Generic;
using System.Text;
using TaskManager.BLL.Interfaces;
using TaskManager.DAL.Interfaces;

namespace TaskManager.BLL.Managers
{

        /// <summary>
        /// class which implements i base service
        /// </summary>
        public abstract class BaseManager : IBaseManager
    {
            /// <summary>
            /// Instance of unit of work
            /// </summary>
            protected readonly IUnitOfWork _unitOfWork;

            /// <summary>
            /// Initializes a new instance of the <see cref="BaseManager" /> class.
            /// </summary>
            /// <param name="unitOfWork"> unit of work instance </param>
            public BaseManager(IUnitOfWork unitOfWork)
            {
                this._unitOfWork = unitOfWork;
            }

            public void Dispose()
            {
                this._unitOfWork.Dispose();
            }
        }
    }
