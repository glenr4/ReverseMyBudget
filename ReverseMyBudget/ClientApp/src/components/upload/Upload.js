// This component is from: https://malcoded.com/posts/react-file-upload/
import React, { Component } from "react";
import Dropzone from "./Dropzone";
import Progress from "./Progress";
import { Dropdown } from "../shared/Dropdown";
import "./Upload.css";
import authService from "../api-authorization/AuthorizeService";

export class Upload extends Component {
  constructor(props) {
    super(props);

    this.state = {
      files: [],
      uploading: false,
      uploadProgress: {},
      successfullUploaded: false,
      selectedAccountId: "",
      accounts: [],
    };
  }

  render() {
    return (
      <div className="Card">
        <div className="Upload">
          <span className="Title">Upload Files</span>
          <Dropdown itemsUrl={"accounts"} itemSelected={this.accountSelected} />
          <div className="Content">
            <div>
              <Dropzone
                onFilesAdded={this.onFilesAdded}
                disabled={
                  this.state.uploading || this.state.successfullUploaded
                }
              />
            </div>
            <div className="Files">
              {this.state.files.map((file) => {
                return (
                  <div key={file.name} className="Row">
                    <span className="Filename">{file.name}</span>
                    {this.renderProgress(file)}
                  </div>
                );
              })}
            </div>
          </div>
          <div className="Actions">{this.renderActions()}</div>
        </div>
      </div>
    );
  }

  onFilesAdded = (files) => {
    this.setState((prevState) => ({
      files: prevState.files.concat(files),
    }));
  };

  uploadFiles = async () => {
    if (!this.state.selectedAccountId) {
      alert("Please select an account first");
      return;
    }

    if (this.state.files.length === 0) {
      alert("Please select a file first");
      return;
    }

    this.setState({ uploadProgress: {}, uploading: true });
    const promises = [];
    this.state.files.forEach((file) => {
      promises.push(this.sendRequest(file));
    });
    try {
      await Promise.all(promises);
      debugger;
      // TODO: this always returns successful even when the server returns an error
      this.setState({ successfullUploaded: true, uploading: false });
    } catch (e) {
      // Not Production ready! Do some error handling here instead...
      debugger;
      // This never gets hit
      this.setState({ successfullUploaded: false, uploading: false });
    }
  };

  sendRequest = (file) => {
    return new Promise(async (resolve, reject) => {
      const req = new XMLHttpRequest();

      req.upload.addEventListener("progress", (event) => {
        if (event.lengthComputable) {
          const copy = { ...this.state.uploadProgress };
          copy[file.name] = {
            state: "pending",
            percentage: (event.loaded / event.total) * 100,
          };
          this.setState({ uploadProgress: copy });
        }
      });

      req.upload.addEventListener("load", (event) => {
        const copy = { ...this.state.uploadProgress };
        copy[file.name] = { state: "done", percentage: 100 };
        this.setState({ uploadProgress: copy });

        debugger;

        resolve(req.response);
      });

      req.upload.addEventListener("error", (event) => {
        const copy = { ...this.state.uploadProgress };
        copy[file.name] = { state: "error", percentage: 0 };
        this.setState({ uploadProgress: copy });

        debugger;
        reject(req.response);
      });

      const formData = new FormData();
      formData.append("file", file, file.name);

      const token = await authService.getAccessToken();

      req.open("POST", `transactions/import/${this.state.selectedAccountId}`);
      req.setRequestHeader("Authorization", `Bearer ${token}`);
      req.send(formData);
    });
  };

  renderProgress = (file) => {
    const uploadProgress = this.state.uploadProgress[file.name];
    if (this.state.uploading || this.state.successfullUploaded) {
      return (
        <div className="ProgressWrapper">
          <Progress progress={uploadProgress ? uploadProgress.percentage : 0} />
          <img
            className="CheckIcon"
            alt="done"
            src="baseline-check_circle_outline-24px.svg"
            style={{
              opacity:
                uploadProgress && uploadProgress.state === "done" ? 0.5 : 0,
            }}
          />
        </div>
      );
    }
  };

  renderActions = () => {
    if (this.state.successfullUploaded) {
      // TODO: Redirect to Transactions page
      alert("Transactions imported successfully");
      return (
        <button
          onClick={() => {
            this.setState({ files: [], successfullUploaded: false });
          }}
        >
          Clear
        </button>
      );
    } else {
      return (
        <button
          disabled={this.state.files.length < 0 || this.state.uploading}
          onClick={this.uploadFiles}
        >
          Upload
        </button>
      );
    }
  };

  accountSelected = (id) => {
    this.setState({ selectedAccountId: id });
  };
}
