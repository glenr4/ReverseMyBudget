// This component is from: https://malcoded.com/posts/react-file-upload/
import React, { Component } from "react";
import Dropzone from "../../shared/upload/Dropzone";
import Progress from "../../shared/upload/Progress";
import { Dropdown } from "../../shared/dropdown/Dropdown";
import "./UploadTransactions.css";
import authService from "../../api-authorization/AuthorizeService";

export class UploadTransactions extends Component {
  constructor(props) {
    super(props);

    this.state = {
      file: "",
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
                disabled={this.state.uploading}
              />
            </div>
            <div className="Files">
              <div className="Row">{this.renderFileList()}</div>
            </div>
          </div>
          <div className="Actions">
            <button
              disabled={!this.state.file || this.state.uploading}
              onClick={this.uploadFiles}
            >
              Upload
            </button>
          </div>
        </div>
      </div>
    );
  }

  renderFileList = () => {
    if (this.state.file) {
      return (
        <>
          <span className="Filename">{this.state.file.name}</span>
          {this.renderProgress(this.state.file)}
        </>
      );
    }
  };

  onFilesAdded = (files) => {
    this.setState({ file: files[0] });
  };

  uploadFiles = async () => {
    if (!this.state.selectedAccountId) {
      alert("Please select an account first");
      return;
    }

    if (!this.state.file) {
      alert("Please select a file first");
      return;
    }

    this.setState({ uploadProgress: {}, uploading: true });

    await this.sendFile(this.state.file);
  };

  sendFile = async (file) => {
    const formData = new FormData();
    formData.append("file", file, file.name);
    const token = await authService.getAccessToken();

    const options = {
      method: "POST",
      body: formData,
      headers: !token ? {} : { Authorization: `Bearer ${token}` },
    };

    await fetch(`transactions/import/${this.state.selectedAccountId}`, options)
      .then((response) => {
        console.log(response);

        this.processResponse(response);
      })
      .catch((reason) => {
        console.log(reason);

        alert("There was an error, please try again later");

        this.setState({ successfullUploaded: false, uploading: false });
      });
  };

  renderProgress = (file) => {
    const uploadProgress = this.state.uploadProgress[file.name];
    if (this.state.uploading) {
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

  accountSelected = (id) => {
    this.setState({ selectedAccountId: id });
  };

  processResponse = (response) => {
    if (response.status < 400) {
      console.log("successful");
      this.setState({
        successfullUploaded: true,
        uploading: false,
        file: "",
      });

      this.props.history.push("/get-transactions");
    } else {
      console.log("error");
      alert("There was an error, please try again later");

      this.setState({ successfullUploaded: false, uploading: false });
    }
  };
}
