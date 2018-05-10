using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum SSActionEventType: int { Started, Completed }
public enum SSActionTargetType : int { Normal, Catching }    //与原来不同的地方

public interface ISSActionCallback {
    void SSActionEvent(SSAction source,
        SSActionEventType eventType = SSActionEventType.Completed,
        SSActionTargetType intParam = SSActionTargetType.Normal,     //动作结束时回调，需要告知是哪种动作
        string strParam = null,
        object objParam = null);
}
