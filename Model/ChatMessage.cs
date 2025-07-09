using System;
using System.Collections.Generic;

namespace DataAccess;

public partial class ChatMessage
{
    public int MessageId { get; set; }

    public int SenderId { get; set; }

    public string SenderRole { get; set; } = null!;

    public int ReceiverId { get; set; }

    public string ReceiverRole { get; set; } = null!;

    public string MessageContent { get; set; } = null!;

    public DateTime? SentAt { get; set; }

    public bool? IsRead { get; set; }

    public virtual SystemUser Receiver { get; set; } = null!;

    public virtual SystemUser Sender { get; set; } = null!;
}
