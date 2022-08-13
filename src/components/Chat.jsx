import React, { useState,useEffect } from "react";
import { useSelector } from "react-redux";
import { HubConnectionBuilder } from "@microsoft/signalr";

const Chat = () => {
  const username = useSelector((state) => state.username);
  const [message, setMessage] = useState("");
  const [chat, setChat] = useState(["test message 1", "test message 2"]);
  console.log(username);

  const [hubCx, setHubCx] = useState(null);

  useEffect(() => {
    (async () => {
      const newConnection = new HubConnectionBuilder()
        .withUrl("https://localhost:7278/chatsocket") // Ensure same as BE
        .withAutomaticReconnect()
        .build();
      await newConnection.start();

      // Let's also setup our receiving method...
      newConnection.on("ReceiveOne", (name, mess) => {
        setChat((c) => [`${name}: ${mess}`, ...c]);
      });
      setHubCx(newConnection);
    })(); // Just a way to run an async func in useEffect...
  }, []);

  const handleSubmit = (e) => {
    e.preventDefault();
    hubCx.invoke('SendMessage1', username, message);
    setMessage('');
  };
  const renderMessages = () => {
    let i = 0;
    return chat.map((m) => <div key={`${i++}`}>{m}</div>);
  };
  return (
    <div>
      <h1>Chat App</h1>
      <div>
        <h2>Send Message</h2>

        <form onSubmit={handleSubmit}>
          <input
            type="text"
            value={message}
            onChange={(e) => setMessage(e.target.value)}
            placeholder="Message"
          />
          &nbsp;
          <input type="submit" value="Submit" />
        </form>
      </div>
      <hr />
      <h2>Chat Log</h2>
      <div>{renderMessages()}</div>
    </div>
  );
};

export default Chat;
